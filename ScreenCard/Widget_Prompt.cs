using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenCard
{
    public class Widget_Prompt : Widget_Object
    {
        public StringBuilder _textBuffer = new StringBuilder();
        private object _textBufferLock = new object();
        private int _currentBufferSize = 0;
        private bool _enterKeyPressed = false;
        private bool _bufferLocked = false;
        private Action<string> _promptAction = null;

        public string TextInput { get; set; }

        public int MaxInputLength { get; set; }

        public Widget_Prompt()
            : base()
        {
            MaxInputLength = 10;
            _textBuffer.Clear();
        }

        private bool HasPromptAction()
        {
            if (_promptAction != null)
            {
                return true;
            }

            return false;
        }

        private void HandlePrompt()
        {
            if (HasPromptAction())
            {
                _promptAction(TextInput);
            }
        }

        public void OnPrompt(Action<string> onPrompt)
        {
            _promptAction = onPrompt;
        }

        #region Rendering of Prompt
        public override void Render()
        {
            if (!_show) return;

            Display.Print(_location.X, _location.Y, "[{0}]", _textBuffer.ToString().PadRight(MaxInputLength).Substring(0, MaxInputLength));

            if (!_hasFocus) return;

            Display.SetTextCursorPosition(_location.X + 1 + _currentBufferSize, _location.Y);

            if (_bufferLocked) return;

            lock (_textBufferLock)
            {
                if (!_bufferLocked)
                {
                    _bufferLocked = true;

                    Display.Print(_location.X, _location.Y, "[{0}]", _textBuffer.ToString().PadRight(MaxInputLength).Substring(0, MaxInputLength));
                    Display.SetTextCursorPosition(_location.X + 1 + _currentBufferSize, _location.Y);

                    Thread _worker = new Thread(() =>
                    {
                        _enterKeyPressed = false;
                        while (!_enterKeyPressed)
                        {
                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo keyInfo = Console.ReadKey();

                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    _bufferLocked = false;
                                    _enterKeyPressed = true;
                                    TextInput = _textBuffer.ToString();
                                    _currentBufferSize = _textBuffer.ToString().TrimEnd(' ').Length;

                                    _textBuffer.Clear();

                                    Display.Print(_location.X, _location.Y, "[{0}]", _textBuffer.ToString().PadRight(MaxInputLength).Substring(0, MaxInputLength));
                                    Display.SetTextCursorPosition(_location.X + 1 + _currentBufferSize, _location.Y);

                                    HandlePrompt();
                                }
                                else if (keyInfo.Key == ConsoleKey.Backspace)
                                {
                                    if (_currentBufferSize > 0)
                                    {
                                        _currentBufferSize = _textBuffer.Length > 0 ? _textBuffer.Length - 1 : 0;

                                        if (_textBuffer.Length > _currentBufferSize)
                                        {
                                            _textBuffer[_currentBufferSize] = ' ';
                                        }
                                    }

                                    string bufferString = _textBuffer.ToString().TrimEnd(' ');

                                    _textBuffer = new StringBuilder(bufferString);

                                    _currentBufferSize = bufferString.Length;

                                    Display.Print(_location.X, _location.Y, "[{0}]", bufferString.PadRight(MaxInputLength).Substring(0, MaxInputLength));
                                    Display.SetTextCursorPosition(_location.X + 1 + _currentBufferSize, _location.Y);
                                }
                                else
                                {
                                    if (_textBuffer.Length < MaxInputLength)
                                    {
                                        _textBuffer.Append(keyInfo.KeyChar.ToString());
                                    }

                                    string bufferString = _textBuffer.ToString();

                                    _textBuffer = new StringBuilder(bufferString);

                                    _currentBufferSize = bufferString.Length;

                                    Display.Print(_location.X, _location.Y, "[{0}]", bufferString.PadRight(MaxInputLength).Substring(0, MaxInputLength));

                                    Display.SetTextCursorPosition(_location.X + 1 + _currentBufferSize, _location.Y);
                                }
                            }
                        }

                    });

                    _worker.Start();
                }
            }
        }

        #endregion

        public override void SetFocus()
        {
            base.SetFocus();

            Display.ShowCursor(true);
        }
    }
}
