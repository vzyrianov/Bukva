using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bukva
{
    class KeyTranslator
    {
        public bool Enabled { get; set; }

        KeyboardHook keyboardHook;
        
        public KeyTranslator()
        {
            keyboardHook = new KeyboardHook();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed; ;
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedEventArgs e)
        {

        }
    }
}
