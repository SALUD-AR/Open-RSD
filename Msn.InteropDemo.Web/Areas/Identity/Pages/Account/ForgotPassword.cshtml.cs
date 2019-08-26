using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Communication.Emailing;
using Msn.InteropDemo.Web.Emailing;

namespace Msn.InteropDemo.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly ILogger<ForgotPasswordModel> _logger;
        private readonly UserManager<Entities.Identity.SystemUser> _userManager;
        private readonly Communication.Emailing.IEmailSender _emailSender;
        private readonly IEmailGenerator _emailGenerator;
        private readonly EmailTemplatesWebPath _emailTemplatesWebPath;
        private readonly EmailSenderConfiguration _emailSenderConfiguration;

        public ForgotPasswordModel(ILogger<ForgotPasswordModel> logger,
                                   UserManager<Entities.Identity.SystemUser> userManager,
                                   Communication.Emailing.IEmailSender emailSender,
                                   Emailing.IEmailGenerator emailGenerator,
                                   Emailing.EmailTemplatesWebPath emailTemplatesWebPath,
                                   Communication.Emailing.EmailSenderConfiguration emailSenderConfiguration)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _emailGenerator = emailGenerator;
            _emailTemplatesWebPath = emailTemplatesWebPath;
            _emailSenderConfiguration = emailSenderConfiguration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "La {0} es requerida.")]
            [EmailAddress(ErrorMessage = "Debe ingresar una {0} válida.")]
            [Display(Name = "Casilla de Correo")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return RedirectToPage("./ForgotPasswordConfirmation");
                //}

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null,
                //    values: new { code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var codEncoded = System.Web.HttpUtility.UrlEncode(code);
                var email = _emailGenerator.GenerateResetPasswordEmail(codEncoded, user.Nombre, user.Apellido, _emailSenderConfiguration.UserName, user.Email);
                _emailSender.SendEmail(email);

                _logger.LogInformation($"Email Password reset send to:{user.Email}");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
