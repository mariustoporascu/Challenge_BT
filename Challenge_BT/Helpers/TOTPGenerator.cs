using System.Security.Cryptography;
using System.Text;

namespace Challenge_BT.Helpers
{
    public static class TOTPGenerator
    {
        public static string Generate(string userId)
        {
            string oneTimePassword = "";
            DateTime dateTime = DateTime.UtcNow;
            Random random = new Random();

            // Build string
            string _randomString = userId;
            _randomString += dateTime.Day.ToString();
            _randomString += dateTime.Month.ToString();
            _randomString += dateTime.Year.ToString();
            _randomString += dateTime.Hour.ToString();
            _randomString += dateTime.Minute.ToString();
            _randomString += dateTime.Second.ToString();
            _randomString += dateTime.Millisecond.ToString();

            // Build TOTP from string
            Console.WriteLine("TOTP value: " + _randomString);
            using (MD5 md5 = MD5.Create())
            {
                // Get hash code of the string in byte format.
                byte[] _stringBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(_randomString));

                // Convert byte array to integer.
                int _stringBytesToInt = BitConverter.ToInt32(_stringBytes, 0);

                // Convert abs value to string
                string _intToString = Math.Abs(_stringBytesToInt).ToString();

                // Check if length of hash code is less than 9.
                // If so, then prepend random int from 0 to 9 untill the lenght becomes 9 characters.
                if (_intToString.Length < 9)
                {
                    StringBuilder sb = new StringBuilder(_intToString);
                    for (int k = 0; k < (9 - _intToString.Length); k++)
                    {
                        sb.Insert(0, random.Next(0, 9));
                    }
                    _intToString = sb.ToString();
                }
                oneTimePassword = _intToString;
            }
            //Adding random letters to the OTP.
            StringBuilder builder = new StringBuilder();
            string randomString = "";
            for (int i = 0; i < 4; i++)
            {
                randomString += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            }
            int randomNumber = random.Next(2, 5);

            //Form alphanumeric OTP and rearrange it reandomly.
            string otpString = randomString.Substring(0, randomNumber);
            otpString += oneTimePassword.Substring(0, 7 - randomNumber);
            oneTimePassword = new string(otpString.ToCharArray().OrderBy(s => (random.Next(2) % 2) == 0).ToArray());

            return oneTimePassword;
        }
    }
}
