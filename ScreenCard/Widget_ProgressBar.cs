using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCard
{
    public class Widget_ProgressBar : Widget_TextBox
    {
        private string _progressText = string.Empty;
        private int _progressValue = 0;

        public string ProgressText
        {
            get
            {
                return _progressText;
            }
            set
            {
                _progressText = value;
                _stateHasChanged = true;
            }
        }

        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                if (_progressValue != value)
                {
                    _stateHasChanged = true;

                    //Display.UpdateLayout();
                }

                _progressValue = Math.Min(100, value);
            }
        }

        public Widget_ProgressBar()
            : base()
        {
            _show = false;
        }

        public override void Render()
        {
            if (!_show)
            {
                _textBuffer.Clear();
            }
            else
            {
                if (_stateHasChanged)
                {
                    SetText(string.Format("{0} {1}%", _progressText, _progressValue).PadRight(20, '.'));
                }
            }

            base.Render();
        }
    }
}
