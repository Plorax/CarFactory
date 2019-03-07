using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public sealed class CrashCatch
    {
        #region Private Static Variables
        private static int _crashIndexCount = -1;
        private static List<Exception> _exceptionStack = new List<Exception>();
        private static object _exceptionStackLock = new object();
        #endregion

        #region Private Variables
        private int _crashIndex = 0;
        private Action<CrashCatch> _tryAction = null;
        private Action<CrashCatch> _onResume = null;
        private Action<CrashCatch> _onCrash = null;
        private bool _hasException = false;
        private StringBuilder _errorLog = new StringBuilder();
        private StringBuilder _infoLog = new StringBuilder();
        private bool _dumpOnCrash = false;
        private bool _quit = false;
        #endregion

        private CrashCatch()
        {

        }

        private CrashCatch(Action<CrashCatch> tryAction)
        {
            this._tryAction = tryAction;
        }

        public string GetErrorLogInformation()
        {
            return _errorLog.ToString();
        }

        public string GetGeneralInformationLog()
        {
            return _infoLog.ToString();
        }

        public void LogInfo(string format, params object [] parameters)
        {
            _infoLog.AppendFormat(format + "\r\n", parameters);
        }

        public void LogError(string format, params object [] parameters)
        {
            _errorLog.AppendFormat(format + "\r\n", parameters);
        }

        public void Quit()
        {
            _quit = true;
        }

        private bool HasResumeCapability()
        {
            if (_onResume != null)
            {
                return true;
            }

            return false;
        }
        private void ResumeAfterCrash()
        {
            if (_onResume != null)
            {
                _onResume(this);
            }
        }

        private bool HasCrashHandler()
        {
            if (_onCrash != null)
            {
                return true;
            }

            return false;
        }


        public CrashCatch OnResume(Action<CrashCatch> onResume)
        {
            this._onResume = onResume;

            return this;
        }

        public CrashCatch OnCrash(Action<CrashCatch> onCrash)
        {
            this._onCrash = onCrash;

            return this;
        }

        public CrashCatch GetException(out Exception exception)
        {
            exception = _exceptionStack[_crashIndex];

            return this;
        }

        public Exception GetException()
        {
            return _exceptionStack[_crashIndex];
        }

        public CrashCatch DumpOnCrash()
        {
            _dumpOnCrash = true;

            return this;
        }

        private void HandleCrash()
        {
            if (_onCrash != null)
            {
                _onCrash(this);
            }
        }

        public CrashCatch Execute()
        {
            try
            {
                if (_tryAction != null)
                {
                    /* execute try code */ _tryAction(this);
                }
            }
            catch (Exception exception)
            {
                _hasException = true;
                _exceptionStack[_crashIndex] = exception;

                if (_dumpOnCrash)
                {
                    LogError("{0}\r\n{1}\r\n", exception.Message, exception.StackTrace);
                }
            }
            finally
            {
                if (_hasException 
                    && HasCrashHandler())
                {
                    HandleCrash();
                }
            }

            if (!_quit && HasResumeCapability())
            {
                ResumeAfterCrash();
            }

            return this;
        }

        public CrashCatch Reset()
        {
            _exceptionStack[_crashIndex] = null;
            _tryAction = null;
            _onResume = null;
            _onCrash = null;
            _hasException = false;
            _errorLog = new StringBuilder();
            _infoLog = new StringBuilder();
            _dumpOnCrash = false;

            return this;
        }

        public static CrashCatch Create()
        {
            CrashCatch crashCatch = new CrashCatch();

            lock (_exceptionStackLock)
            {
                _exceptionStack.Add(null);
                crashCatch._crashIndex = _exceptionStack.Count() - 1;
            }

            return crashCatch;
        }

        public CrashCatch Try(Action<CrashCatch> tryAction)
        {
            this._tryAction = tryAction;

            return this;
        }
    }
}
