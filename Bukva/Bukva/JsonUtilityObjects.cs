using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bukva
{
    public struct KeyPress
    {
        public string key { get; set; }
        public bool isShiftDown { get; set; }
        public bool isCtrlDown { get; set; }


        public void SetModifiersFromCurrentState()
        {
            isCtrlDown = Control.ModifierKeys.HasFlag(Keys.Control);
            isShiftDown = Control.ModifierKeys.HasFlag(Keys.Control);
        }
    }

    public struct KeyTranslation
    {
        public string output { get; set; }
        public List<KeyPress> input { get; set; }
    }

    public struct Bind
    {
        public string key { get; set; }
        public bool isShiftDown { get; set; }
        public bool isCtrlDown { get; set; }
    }

    public struct Shortcut
    {
        public string commandName { get; set; }
        public Bind bind { get; set; }
    }

    public struct Configuration
    {
        public List<KeyTranslation> keyTranslations { get; set; }
        public List<Shortcut> shortcuts { get; set; }
    }
}
