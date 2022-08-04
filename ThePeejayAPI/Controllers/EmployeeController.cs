using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ThePeejayAPI.Models;
using ThePeejayAPI.Repositories;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ApplicationUser> logger;
        private readonly IConfiguration config;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager
                                 , ILogger<ApplicationUser> logger, IConfiguration config, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.config = config;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Employee()
        {
            return Ok(userManager.Users.ToList());
        }

        [HttpGet("id")]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Department = model.Department,
                        Gender = model.Gender,
                        CreatedDate = DateTime.UtcNow,
                        City = model.City,
                        Photo = model.Photo,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                    var userFound = await userManager.FindByNameAsync(user.UserName);

                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        UriBuilder uriBuilder = new UriBuilder(config["ReturnPath:ConfirmEmail"]);
                        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                        uriBuilder.Query = query.ToString();
                        var urlString = uriBuilder.ToString();

                        string senderEmail = config["ReturnPath:SenderEmail"];

                        await emailSender.SendEmail(senderEmail, userFound.Email, urlString, "<h1>Please confirm your mail</h2>");
                    }

                    return CreatedAtAction("Employee", new { id = user.Id }, user);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] EmployeeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Department = model.Department,
                        Gender = model.Gender,
                        CreatedDate = DateTime.UtcNow,
                        City = model.City,
                        Photo = model.Photo,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                    var userFound = await userManager.FindByNameAsync(user.UserName);

                    //if (result.Succeeded)
                    //{
                    //    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //    UriBuilder uriBuilder = new UriBuilder(config["ReturnPath:ConfirmEmail"]);
                    //    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    //    uriBuilder.Query = query.ToString();
                    //    var urlString = uriBuilder.ToString();

                    //    string senderEmail = config["ReturnPath:SenderEmail"];

                    //    await emailSender.SendEmail(senderEmail, userFound.Email, urlString, "<h1>Please confirm your mail</h2>");
                    //}
                    
                    if(!await roleManager.RoleExistsAsync(EmployeeRoles.Admin))
                    {
                        await roleManager.CreateAsync(new IdentityRole(EmployeeRoles.Admin));
                    }

                    if (!await roleManager.RoleExistsAsync(EmployeeRoles.Others))
                    {
                        await roleManager.CreateAsync(new IdentityRole(EmployeeRoles.Others));
                    }

                    if(await roleManager.RoleExistsAsync(EmployeeRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(user, EmployeeRoles.Admin);
                    }

                    return CreatedAtAction("Employee", new { id = user.Id }, user);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if(user!=null&&await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: config["JWT:ValidIssuer"],
                    audience: config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1.0),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return Unauthorized();
        }
    }
}

