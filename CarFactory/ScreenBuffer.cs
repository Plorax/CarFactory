using Core.Base;
using System;
using System.Text;
using System.Drawing;

namespace CarFactory
{
    public abstract class ScreenBuffer : IScreenBuffer
    {
        protected StringBuilder _buffer = null;
        private object _screenBufferInitHandle = new object();
        private object _screenBufferFlip = new object();

        public virtual void Clear()
        {
            Console.Clear();
            _buffer.Clear();
        }

        public virtual void Initialize(int numberOfLines, int charactersInLine)
        {
            if (_buffer == null)
            {
                lock (_screenBufferInitHandle)
                {
                    if (_buffer == null)
                    {
                        _buffer = new StringBuilder();

                        // Set size of string builder buffer
                        _buffer.Capacity = numberOfLines * charactersInLine;

                        // Hide cursor by default
                        ShowCursor(false);
                    }
                }
            }
        }

        public virtual void Render()
        {
            Console.Clear();
            Console.Write(_buffer.ToString());
        }

        public virtual void Shutdown()
        {
            // clear the text buffer
            _buffer.Clear();

            // clear the console display
            Console.Clear();
        }

        public virtual void Suspend()
        {
            Console.Clear();
        }

        public virtual void SetFocus(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public virtual void DrawText(int x, int y, string format, params object [] parameters)
        {
            lock (_screenBufferFlip)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(format, parameters);
            }
        }

        public void SetColour(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public virtual string AwaitKeyboardInput()
        {
            return Console.ReadLine();
        }

        public virtual void SetTextCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public virtual void ShowCursor(bool show)
        {
            if (Console.CursorVisible != show)
            {
                Console.CursorVisible = show;
            }
        }

        public virtual Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }
    }
}
