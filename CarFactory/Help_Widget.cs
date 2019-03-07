using ScreenCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    public class Help_Widget : Widget_Object
    {
        public Help_Widget()
            : base()
        {
            _stateHasChanged = true;
        }

        public override void Render()
        {
            if (_show && _stateHasChanged)
            {
                string[] helpCommands = { "Type [quit] - to exit" };

                for (int i = 0; i < helpCommands.Length; ++i)
                {
                    Display.Print(_location.X, _location.Y + i, "{0}", helpCommands[i]);
                }

                _stateHasChanged = false;
            }
        }
    }
}
