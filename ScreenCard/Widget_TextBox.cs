using Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScreenCard
{
    public class Widget_TextBox : Widget_Object
    {
        private Widget_Square _border = new Widget_Square();
        protected StringBuilder _textBuffer = new StringBuilder();

        public string Text { get; set; }

        public bool DrawBorder { get; set; }

        public Widget_TextBox()
            : base()
        {
            _stateHasChanged = true;
        }

        public override void Render()
        {
            if (_show && _stateHasChanged)
            {
                _border.SetLocation(this.Location.X, this.Location.Y);
                _border.SetDimensions(_width, _height);

                if (DrawBorder)
                {
                    _border.Render();
                }

                // Set text colour
                Display.SetColour(_colour);

                SetContent(_textBuffer.ToString());

                // Display textbox
                Display.Print(_border.Location.X + 2, _border.Location.Y + 1, (string)GetContent());

                _stateHasChanged = false;
            }
        }

        public void AppendText(string format, params object[] parameters)
        {
            _textBuffer.AppendFormat(format, parameters);
        }

        public void SetText(string format, params object[] parameters)
        {
            string displayText = string.Format(format, parameters);

            int promptIndex = displayText.IndexOf("_");

            displayText = displayText.Replace("_", "");

            _textBuffer.Clear();
            _textBuffer.Append(displayText);

            //Display.SetTextCursorPosition(_location.X + promptIndex, _location.Y);
        }

        public void AcceptInput()
        {
            Text = Display.AwaitKeyboardInput();
        }
    }
}
