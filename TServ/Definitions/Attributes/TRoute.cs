using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TServ.Definitions.Attributes
{
    public class TRoute : Attribute
    {
        public string Route = string.Empty;

        public TRoute(string route) : base()
        {
            if (String.IsNullOrWhiteSpace(route))
                throw new ArgumentException("TRoute(string) - Route string cannot be null nor empty nor whitespace.");
            if (!Uri.IsWellFormedUriString(route, UriKind.Relative))
                throw new ArgumentException("TRoute(string) - Route string is not useable as an uri path.");
            if (route.First().Equals('/'))
                route = route.TrimStart('/');
            if (!route.Last().Equals('/'))
                route = $"{route}/";
            Route = route;
        }
    }
}
