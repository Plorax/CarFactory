using Core.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCard
{
    public class Widget_Square : Widget_Object
    {
        private StringBuilder _buffer = new StringBuilder();

        public Widget_Square()
            : base()
        {
            _stateHasChanged = true;
        }

        public override void Render()
        {
            if (_stateHasChanged)
            {
                Update();

                Display.SetColour(_borderColour);
                Display.Print(_location.X, _location.Y, "{0}", _buffer.ToString());

                _stateHasChanged = false;
            }
        }

        public override void SetColour(ConsoleColor color)
        {
            _borderColour = color;
            _stateHasChanged = true;
        }

        public override void SetDimensions(int width, int height)
        {
            _width = width;
            _height = height;
            _stateHasChanged = true;
        }

        public override void SetFocus()
        {
            _hasFocus = true;
            _stateHasChanged = true;
        }

        public override void SetLocation(int x, int y)
        {
            _location = new Point(x, y);
        }

        public override bool StateHasChanged()
        {
            return _stateHasChanged;
        }

        public void Update()
        {
            _buffer.Clear();
            _buffer.Append("╔");

            string space = "";
            string temp = "";

            for (int i = 0; i < _width; i++)
            {
                space += " ";
                _buffer.Append("═");
            }

            for (int j = 0; j < _location.X; j++)
                temp += " ";

            _buffer.Append("╗" + "\n");

            for (int i = 0; i < _height; i++)
                _buffer.Append(temp + "║" + space + "║" + "\n");

            _buffer.Append(temp + "╚");

            for (int i = 0; i < _width; i++)
                _buffer.Append("═");

            _buffer.Append("╝" + "\n");
        }

    }
}
