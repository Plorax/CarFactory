using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public interface IScreenWidget
    {
        void SetFocus();

        void Render();

        bool StateHasChanged();

        void SetLocation(int x, int y);

        void SetDimensions(int width, int height);

        void SetColour(ConsoleColor color);

        void SetContent(object content);

        object GetContent();

        void SetBorderColour(ConsoleColor color);
    }
}
