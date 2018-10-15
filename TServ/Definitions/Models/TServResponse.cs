using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TServ.Definitions.Models
{
    public struct TServResponse
    {
        public string Message;
        public HttpStatusCode Status;
        public TServResponseType Type;
    }

    public enum TServResponseType
    {
        UNDEFINED = -1,
        RAW_TEXT = 1,
        HTML = 1,
        EMAIL = 2,
        JOB_REPORT = 3
    }
}
