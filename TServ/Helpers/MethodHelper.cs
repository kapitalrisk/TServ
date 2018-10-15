using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TServ.Definitions.Attributes;

namespace TServ.Helpers
{
    public static class MethodHelper
    {
        public static TRoute GetTRouteAttribute(this Delegate method)
        {
            var routeObj = method.Method.GetCustomAttributes(typeof(TRoute), false).Where(x => x.ToString().Equals("TServ.Definitions.Attributes.TRoute")).FirstOrDefault();
            return routeObj != null ? ((TRoute)routeObj) : null;
        }
    }
}
