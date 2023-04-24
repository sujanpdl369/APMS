using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Common.LocalModel
{
    public static class Utility
    {
        public static DateTime GetCurrentDateTime() => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(TIME_ZONES.EST));

        public static string EncryptMD5(string password, bool appendDay = false)
        {
            MD5 md5 = MD5.Create("md5");
            Encoding utF8 = Encoding.UTF8;
            string s;
            if (!appendDay)
            {
                s = password;
            }
            else
            {
                DateTime dateTime = Utility.GetCurrentDateTime();
                dateTime = dateTime.ToUniversalTime();
                s = dateTime.Day.ToString() + password;
            }
            byte[] bytes = utF8.GetBytes(s);
            byte[] hash = md5.ComputeHash(bytes);
            string str = "";
            for (int index = 0; index < hash.Length; ++index)
                str += hash[index].ToString("x2").ToUpperInvariant();
            return str;
        }

        public static class TIME_ZONES
        {
            /// <summary>
            /// Pacific Standard Time
            ///  (for Pacific Time (US & Canada))
            /// </summary>
            public static String PST = "Pacific Standard Time";
            /// <summary>
            /// Mountain Standard Time
            ///  (for Mountain Time (US & Canada))
            /// </summary>
            public static String MST = "Mountain Standard Time";
            /// <summary>
            /// Central Standard Time
            /// (for Central Time (US & Canada))
            /// </summary>
            public static String CST = "Central Standard Time";
            /// <summary>
            /// Eastern Standard Time
            /// (for Eastern Time (US & Canada))
            /// </summary>
            public static String EST = "Eastern Standard Time";
        }

        public static bool validatePassword(string password, string hashpassword) => BCrypt.Net.BCrypt.Verify(password, hashpassword);
    }
}
