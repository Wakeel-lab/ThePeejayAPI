using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ThePeejayAPI.Models;
using ThePeejayAPI.Repositories;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<ApplicationUser> logger;
        private readonly IConfiguration config;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<ApplicationUser> logger,
            IConfiguration config, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.config = config;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Accounts()
        {
            return Ok(userManager.Users.ToList());
        }

        [HttpGet("id:string")]
        public async Task<IActionResult>Accounts(string id)
        {
            return Ok(await userManager.FindByIdAsync(id));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]CustomerModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                var identityUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(identityUser, user.Password);
                var userFound = await userManager.FindByNameAsync(identityUser.UserName);

                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                        UriBuilder uriBuilder = new UriBuilder(config["ReturnPath:ConfirmEmail"]);
                        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                        uriBuilder.Query = query.ToString();
                        var urlString = uriBuilder.ToString();

                        string senderEmail = config["ReturnPath:SenderEmail"];

                        await emailSender.SendEmail(senderEmail, userFound.Email, urlString, "<h1>Please confirm your mail</h2>");
                    }
                    return CreatedAtAction(nameof(Accounts), new { id = identityUser.Id }, identityUser);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            UserLogin userLogin = new UserLogin()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return Ok(userLogin);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await this.userManager.FindByEmailAsync(userLogin.Email);

                    if (user!=null &&!user.EmailConfirmed && (await this.userManager.CheckPasswordAsync(user, userLogin.Password)))
                    {
                        ModelState.AddModelError("", "Email not confirmed yet.");
                        return Ok(userLogin);
                    }

                    var result = await this.signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }

                        return RedirectToAction();
                    }

                    
                }
                ModelState.AddModelError("", "Invalid login attempt");

                return Ok(userLogin);
                
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            

        }

        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("index", "home");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "account",
                                                new { ReturnUrl = returnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [Route("ExternalLoginCallBack")]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            UserLogin userLogin = new UserLogin()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");

                return StatusCode(StatusCodes.Status400BadRequest, userLogin);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");
                return BadRequest(userLogin);
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return RedirectToAction(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                return Problem();
            }
        }
    }
}
