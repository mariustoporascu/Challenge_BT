using Challenge_BT.Models;

namespace Challenge_BT.Helpers
{
    public static class TOTPStorage
    {
        private static List<TOTP> _TOTPs = new List<TOTP>();
        public static void Add(TOTP totp) => _TOTPs.Add(totp);
        public static TOTP GetTOTP(string userId) => _TOTPs.LastOrDefault(otp => otp.UserId == userId);
        public static bool CheckHasValidOTP(string userId) => _TOTPs.LastOrDefault(otp => otp.UserId == userId) != null &&
            _TOTPs.LastOrDefault(otp => otp.UserId == userId).ExpiryTime.CompareTo(DateTime.UtcNow) >= 0;
    }
}
