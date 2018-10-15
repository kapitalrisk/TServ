using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TServ.Definitions.Configuration;

namespace TServ.Helpers
{
    public static class StringHelper
    {
        public static string GetLocalAddress(this string pref) => $"{TServDefaults.LocalAdress}{pref}";
    }
}
