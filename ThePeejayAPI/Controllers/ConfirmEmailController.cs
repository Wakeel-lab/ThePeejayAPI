using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmEmailController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public ConfirmEmailController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirm model)
        {
            try
            {
                if (model.UserId == null || model.UserToken == null)
                {
                    return Problem("Email confirmation link already tampered with or expired.");
                }

                var user = await userManager.FindByIdAsync(model.UserId);

                if (user == null)
                {
                    return NotFound(
                        $"User with the {user.Id} cannot be found.");
                }

                var result = await userManager.ConfirmEmailAsync(user, model.UserToken);

                if (result.Succeeded)
                {
                    return Ok("Email successfully confirmed. You may now login.");
                }

                return NotFound("Email cannot be confirmed.");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
