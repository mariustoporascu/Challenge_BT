using Challenge_BT.Helpers;
using Challenge_BT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Challenge_BT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpContextAccessor httpContextAccessor, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public TOTP DisplayTOTP { get; set; }

        public string UserIdFromCookie()
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];
            if (cookieValue != null)
            {
                return cookieValue;
            }
            else
            {
                var userId = Guid.NewGuid().ToString();
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("anonymousUsr", userId, option);
                return userId;
            }
        }
        public void OnGet()
        {
            var userId = UserIdFromCookie();
            if (!TOTPStorage.CheckHasValidOTP(userId))
            {
                var otpPass = TOTPGenerator.Generate(userId);
                TOTPStorage.Add(new TOTP
                {
                    UserId = userId,
                    GeneratedTOTP = otpPass,
                    ExpiryTime = DateTime.UtcNow.AddSeconds(30)
                });
            }
            DisplayTOTP = TOTPStorage.GetTOTP(userId);
            _logger.Log(LogLevel.Information, $"Generated TOPT: {DisplayTOTP.GeneratedTOTP}, Expiration Time: {DisplayTOTP.ExpiryTime}");
        }
    }
}