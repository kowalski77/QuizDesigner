using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuizDesigner.Blazor.Server.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] 
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!(this.Email == "quizadmin@email.com" && this.Password == "Questions,21"))
            {
                return this.Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "QuizAdmin"),
                new(ClaimTypes.Email, this.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)).ConfigureAwait(true);

            return this.LocalRedirect(this.Url.Content("~/"));
        }
    }
}