using System.IO;
using AspCore7_Web_Identity.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AspCore7_Web_Identity.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<UsersController> _logger;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public UsersController(SignInManager<Users> signInManager, ILogger<UsersController> logger)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public async Task<IActionResult> Login(string? returnUrl, InputModel? Input)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok("Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Ok("Invalid model.");
        }
        public async Task<IActionResult> LoginApi(string? returnUrl,  string? username,string? password,bool rememberme, string? username1)
        {

            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                var result = await _signInManager.PasswordSignInAsync( username, password, rememberme, lockoutOnFailure: false);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Json(new ReturnData() { code=200,message="success"});
                }
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    return Json(new ReturnData() { code = 200, message = "success" });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    //return RedirectToPage("./Lockout");
                    return Json(new ReturnData() { code = 407, message = "User account locked out" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //return Ok("Invalid login attempt.");
                    return Json(new ReturnData() { code = 401, message = "Invalid login attempt" });
                }
            }

            // If we got this far, something failed, redisplay form
            return Json(new ReturnData() { code = 406, message = "Invalid model" });
        }
        public async Task<IActionResult> LogoutApi(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return Redirect("/Home/Index");
            }
        }
        
        public class ReturnData { 
            public int code { get; set; }
            public string? message { get; set; }
        }
    }
}
