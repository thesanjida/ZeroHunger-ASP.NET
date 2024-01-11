using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BC = BCrypt.Net.BCrypt;

namespace ZeroHunger.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BC.HashPassword(password, 12);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BC.Verify(password, hash);
        }
    }
}