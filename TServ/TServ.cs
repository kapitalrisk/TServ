using System;
using System.Collections.Generic;
using System.Linq;
using TServ.Business;
using TServ.Definitions.Attributes;
using TServ.Definitions.Models;
using TServ.Helpers;

namespace TServ
{
    public sealed class TServ : IDisposable
    {
        #region Singleton & ctor
        private static TServ _instance = null;

        private TServ()
        { }

        public static TServ Instance
        {
            set { _instance = value; }
            get
            {
                if (_instance == null)
                    _instance = new TServ();
                return _instance;
            }
        }
        #endregion

        #region Properties
        private TListener _listener = new TListener();
        private List<Func<TServResponse>> _getListeners = new List<Func<TServResponse>>();
        #endregion

        #region Public (Start / Stop)
        public void Configure()
        {

        }

        public void Start() => _listener.Start();

        public void Stop()
        {
            _listener.Dispose();
            _listener = null;
            this.Dispose();
        }
        #endregion

        #region Private internal
        private void CheckInputMethod(Func<TServResponse> method, string callerName)
        {
            if (method == null)
                throw new ArgumentNullException($"{callerName} - Method cannot be null.");
            if (_getListeners.Contains(method))
                throw new ArgumentException($"{callerName} - Method {method.Method.Name} is already present.");
            if (String.IsNullOrWhiteSpace(method.GetTRouteAttribute().Route))
                throw new ArgumentException($"{callerName} - Method {method.Method.Name} does not have a TRoute attribute defined.");
        }

        private void CheckInputMethod(List<Func<TServResponse>> methods, string callerName)
        {
            foreach (var method in methods)
                this.CheckInputMethod(method, callerName);
        }
        #endregion

        #region Public (Dispose)
        public void Dispose()
        {
            _getListeners?.Clear();
            _listener?.Dispose();
        }
        #endregion

    }
}
