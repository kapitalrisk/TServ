using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TServ.Helpers;

namespace TServ.Business
{
    internal class TListener : IDisposable
    {
        #region Properties
        private HttpListener _listener = new HttpListener();
        private TCallback _callback = new TCallback();
        private CancellationTokenSource Tokenizer = new CancellationTokenSource();
        private List<string> uriPrefixes = new List<string>();
        #endregion

        #region Internal (Business)
        internal void AddPrefixes(IEnumerable<string> prefs) => Parallel.ForEach(prefs, (pref) => { uriPrefixes.Add(pref.GetLocalAddress()); });
        internal void AddPrefixesToListener() => Parallel.ForEach(uriPrefixes, (pref) => { _listener.Prefixes.Add(pref.GetLocalAddress()); });
        internal void ClearListeners() => uriPrefixes.Clear();
        #endregion

        #region Internal (Start / Stop)
        internal void Start()
        {
            Console.WriteLine("TListener - Start requested");

            if (_listener.IsListening)
                _listener.Stop();
            if (!Tokenizer.IsCancellationRequested)
                Tokenizer.Cancel();
            _listener = new HttpListener();
            this.AddPrefixesToListener();

            if (_listener.Prefixes.Count > 0 && !Tokenizer.IsCancellationRequested)
            {
                _listener.Start();
                this.WaitRequests();
            }
        }

        internal void Stop()
        {
            Console.WriteLine("TListener - Stop requested");

            if (!Tokenizer.IsCancellationRequested)
                Tokenizer.Cancel();
            if (_listener.IsListening)
                _listener.Stop();
            ClearListeners();
            _listener?.Close();
        }
        #endregion

        #region Private (Business)
        private void WaitRequests()
        {
            IAsyncResult res = _listener.BeginGetContext(new AsyncCallback(_callback.CallBackRequest), _listener);
            var t = new Task(() =>
            {
                Console.WriteLine("TListener.WaitRequests() - waiting for next request");

                if (!Tokenizer.IsCancellationRequested)
                    res.AsyncWaitHandle.WaitOne(); // blocking
            }, Tokenizer.Token);
            t.Start();
            Console.WriteLine("TListener.WaitRequests() - Waiting for requests terminated");
        }
        #endregion

        #region Public (Dispose)
        public void Dispose() => this.Stop();
        #endregion
    }
}
