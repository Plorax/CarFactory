using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public interface IScreenBuffer
    {
        void Initialize(int numberOfLines, int charactersInLine);

        void Clear();

        void Shutdown();

        void Suspend();

        void SetFocus(int x, int y);

        void DrawText(int x, int y, string format, object[] parameters);

        void SetColour(ConsoleColor color);

        string AwaitKeyboardInput();

        void SetTextCursorPosition(int x, int y);

        void ShowCursor(bool show);

        Point GetCursorPosition();
    }
}
