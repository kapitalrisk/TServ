using System;
using System.IO;
using System.Net;
using System.Text;
using TServ.Definitions.Attributes;
using TServ.Definitions.Models;

namespace TServ
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMethods tst = new TestMethods();
            var key = ConsoleKey.A;

            while (key != ConsoleKey.Q)
            {
                Console.WriteLine("--- TServ Toaster ---");
                Console.WriteLine("1 -> Add isDead listener");
                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        TServ.Instance.Configure();
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public class TestMethods
    {
        [TRoute("Toast/isAlive")]
        public TServResponse GetIsAlive()
        {
            return new TServResponse { Message = "IT IS", Status = HttpStatusCode.OK, Type = TServResponseType.RAW_TEXT };
        }

        [TRoute("Toast/isDead")]
        public TServResponse GetIsDead()
        {
            return new TServResponse { Message = "IT IS NOT", Status = HttpStatusCode.OK, Type = TServResponseType.RAW_TEXT };
        }
    }

    public class SampleCode
    {
        public void Sample()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:80/Toast/isAlive/"); // tested on variety of ports but port 80 is compliant to https://tools.ietf.org/html/rfc4960
            listener.Start();

            while (true)
            {
                var currentContext = listener.GetContext();
                HttpListenerRequest catchedRequest = currentContext.Request;
                HttpListenerResponse response = currentContext.Response;

                Console.WriteLine($"-> Request received : {catchedRequest.RawUrl}");
                response.StatusCode = (int)HttpStatusCode.OK;
                string responseString = "<html><body>OK</body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                using (Stream output = response.OutputStream)
                    output.Write(buffer, 0, buffer.Length);
            }
            //listener.Stop();
        }
    }
}
