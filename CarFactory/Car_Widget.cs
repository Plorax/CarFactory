using ScreenCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    public class Car_Widget : Widget_Object
    {
        public Car_Widget()
            : base()
        {
            _stateHasChanged = true;
        }

        public override void Render()
        {
            if (_show && _stateHasChanged)
            {
                string[] carArray = { "Ferrari", "Laborghini", "Porche", "Audi" };
                ConsoleColor[] carColours = { ConsoleColor.Red, ConsoleColor.DarkMagenta, ConsoleColor.Blue, ConsoleColor.Gray };

                for (int i = 0; i < carArray.Length; ++i)
                {
                    Display.SetColour(carColours[i]);
                    Display.Print(_location.X, _location.Y + i, "{0} - {1}", i + 1, carArray[i]);
                }

                _stateHasChanged = false;
            }
        }
    }
}
