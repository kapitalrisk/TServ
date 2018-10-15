using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TServ.Business
{
    internal class TCallback
    {
        internal void CallBackRequest(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext currentContext = listener.EndGetContext(result);
            Task.Run(() => this.SendCallBack(currentContext));
            return;
        }

        private void SendCallBack(HttpListenerContext currentContext)
        {
            HttpListenerRequest catchedRequest = currentContext.Request;
            HttpListenerResponse response = currentContext.Response;

            Console.WriteLine($"-> Request received : {catchedRequest.RawUrl}");
            response.StatusCode = (int)HttpStatusCode.OK;   // FIXME parametise it
            string responseString = "<html><body>OK</body></html>"; // SELECT CORRECT METHOD HERE
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;

            using (Stream output = response.OutputStream)
                output.Write(buffer, 0, buffer.Length);
        }
    }
}
