using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Web.CustomValidators;

namespace Msn.InteropDemo.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Entities.Identity.SystemUser> _userManager;
        private readonly SignInManager<Entities.Identity.SystemUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<Entities.Identity.SystemUser> userManager,
            SignInManager<Entities.Identity.SystemUser> signInManager,
            IEmailSender emailSender, 
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "La {0} es requerida.")]
            [EmailAddress(ErrorMessage = "Debe ingresar una {0} válida.")]
            [Display(Name = "Casila de Correo")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Teléfono")]
            public string PhoneNumber { get; set; }

            [Display(Name = "CUIT")]
            [Required(ErrorMessage = "El CUIT es requerido")]
            [CuitValidator(ErrorMessage = "El CUIT ingresado es inválido")]
            public string CUIT { get; set; }

            [Display(Name = "Apellido")]
            [Required(ErrorMessage = "El Apellido es requerido")]
            public string Apellido { get; set; }

            [Display(Name = "Nombre")]
            [Required(ErrorMessage = "El Nombre es requerido")]
            public string Nombre { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool mustCongifAccount = false)
        {
            ViewData.Add("MustCongifAccount", mustCongifAccount);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                CUIT = Common.Utils.Helpers.Cuit.ToUIFormat(user.CUIT),
                Apellido = user.Apellido,
                Nombre = user.Nombre
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            user.CUIT = long.Parse(Common.Utils.Helpers.Cuit.ToCleanFormat(Input.CUIT));
            user.Apellido = Input.Apellido;
            user.Nombre = Input.Nombre;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su cuenta ha sido actualizada.";

            _logger.LogInformation($"Usuario modificó sus datos Id:{user.Id}");

            return RedirectToPage(new { mustCongifAccount = false });
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
