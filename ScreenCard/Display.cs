using Core.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenCard
{
    public static class Display
    {
        #region Private Static Variables

        /// <summary>
        /// Points to the ScreenBuffer for our Display
        /// </summary>
        private static IScreenBuffer _screenBuffer = null;

        /// <summary>
        /// Points to the Locking Handle so that we may keep tabs on when the screenBuffer
        /// is switched.
        /// </summary>
        private static object _lockHandle = new object();

        private static Thread _realTimeRender = new Thread(() =>
        {
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= 1)
                {
                    startTime = DateTime.Now;

                    lock (_realTimeObjectLock)
                    {
                        foreach (Widget_Object widgetObject in _realTimeObjects)
                        {
                            widgetObject.Render();
                        }
                    }
                }
            }

        });

        private static List<Widget_Object> _widgetList = new List<Widget_Object>();
        private static object _widgetListObjectLock = new object();

        private static List<Widget_Object> _realTimeObjects = new List<Widget_Object>();
        private static object _realTimeObjectLock = new object();
        #endregion

        public static void Initialize(IScreenBuffer screenBuffer = null)
        {
            lock (_lockHandle)
            {
                lock (_realTimeObjectLock)
                {
                    _realTimeObjects.Clear();
                }

                _screenBuffer = screenBuffer;

                _screenBuffer.Initialize(100, 600);
            }
        }

        public static void Release()
        {
            _realTimeRender.Abort();
            _realTimeObjects.Clear();
            _widgetList.Clear();
            _realTimeRender = null;

            _screenBuffer.Shutdown();
            _screenBuffer = null;
        }

        public static void Print(int x, int y, string format, params object [] parameters)
        {
            _screenBuffer.DrawText(x, y, format, parameters);
        }

        public static void SetColour(ConsoleColor color)
        {
            _screenBuffer.SetColour(color);
        }

        internal static void SetTextCursorPosition(int x, int y)
        {
            _screenBuffer.SetTextCursorPosition(x, y);
        }

        public static string AwaitKeyboardInput()
        {
            return _screenBuffer.AwaitKeyboardInput();
        }

        public static void AttachToLive(Widget_Object widgetObject)
        {
            lock (_realTimeObjectLock)
            {
                _realTimeObjects.Add(widgetObject);

                if (_realTimeRender.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    _realTimeRender.SetApartmentState(ApartmentState.MTA);
                    _realTimeRender.Start();
                }
            }
        }

        public static void AttachWidget(Widget_Object widgetObject)
        {
            lock (_widgetListObjectLock)
            {
                _widgetList.Add(widgetObject);
            }
        }

        public static void ShowCursor(bool show)
        {
            _screenBuffer.ShowCursor(show);
        }

        public static void UpdateLayout()
        {
            _screenBuffer.Clear();

            foreach (var widget in _widgetList)
            {
                widget.Invalidate();
            }
        }
    }
}
