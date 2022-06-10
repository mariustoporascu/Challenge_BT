namespace Challenge_BT.Models
{
    public class TOTP
    {
        public string UserId { get; set; }
        public string GeneratedTOTP { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
