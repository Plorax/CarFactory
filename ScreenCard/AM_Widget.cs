using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCard
{
    public class AM_Widget : Widget_TextBox
    {
        private string _lastAMPM = string.Empty;

        public enum AMPM
        {
            AM = 0,
            PM = 1
        }

        public AM_Widget()
            : base()
        {
            _lastAMPM = DateTime.Now.ToString("hh:mm:ss tt");
        }

        public AMPM GetAMPM()
        {
            string ampm = DateTime.Now.ToString("tt").ToUpper();

            return (ampm == "AM" ? AMPM.AM : AMPM.PM);
        }

        public override void Render()
        {
            string currentAMPM = DateTime.Now.ToString("hh:mm:ss tt");

            if (!_lastAMPM.Equals(currentAMPM))
            {
                _lastAMPM = currentAMPM;
                SetText(currentAMPM);

                _stateHasChanged = true;
            }

            base.Render();
        }
    }
}
