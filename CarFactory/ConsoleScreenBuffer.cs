using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    public class ConsoleScreenBuffer : ScreenBuffer
    {
        private ConsoleScreenBuffer()
            : base()
        {

        }

        public static ScreenBuffer Create()
        {
            ConsoleScreenBuffer consoleScreenBuffer = new ConsoleScreenBuffer();

            return consoleScreenBuffer;
        }
    }
}
