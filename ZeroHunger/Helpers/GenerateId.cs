using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.Helpers
{
    public class GenerateId
    {
        public static string MakeId()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}