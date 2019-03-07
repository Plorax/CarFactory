using Core.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenCard
{
    public abstract class Widget_Object : IScreenWidget
    {
        public Point Location { get { return _location; } set { _location = value; } }
        
        protected bool _live = false;
        protected Point _location = new Point();
        protected ConsoleColor _colour = ConsoleColor.White;
        protected bool _stateHasChanged = false;
        protected int _width = 0;
        protected int _height = 0;
        protected bool _hasFocus = false;
        protected object _content = null;
        protected ConsoleColor _borderColour = ConsoleColor.Green;
        protected bool _show = true;

        public Widget_Object()
        {
            Display.AttachWidget(this);
        }

        public void Show(bool show)
        {
            _show = show;

            Invalidate();
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(_location, new Size(_width, _height));
        }

        public bool PointInBounds(Point point)
        {
            Rectangle bounds = GetBounds();
            if (point.X >= bounds.X
                && point.Y >= bounds.Y
                && point.X <= bounds.Right
                && point.Y <= bounds.Bottom)
            {
                return true;
            }

            return false;
        }

        private bool IsLive()
        {
            return _live;
        }

        public void MakeLive()
        {
            _live = true;

            Display.AttachToLive(this);
        }

        public virtual object GetContent()
        {
            return _content;
        }

        public abstract void Render();

        public virtual void SetColour(ConsoleColor color)
        {
            _stateHasChanged = true;
            _colour = color;
        }

        public virtual void SetContent(object content)
        {
            _stateHasChanged = true;
            _content = content;
        }

        public virtual void SetDimensions(int width, int height)
        {
            _stateHasChanged = true;
            _width = width;
            _height = height;
        }

        public virtual void SetFocus()
        {
            _hasFocus = true;
            _stateHasChanged = true;
        }

        public virtual void SetLocation(int x, int y)
        {
            Location = new Point(x, y);

            _stateHasChanged = true;
        }

        public virtual bool StateHasChanged()
        {
            return _stateHasChanged;
        }

        public virtual void SetBorderColour(ConsoleColor color)
        {
            _borderColour = color;
            _stateHasChanged = true;
        }

        public void Invalidate()
        {
            _stateHasChanged = true;
        }
    }
}
