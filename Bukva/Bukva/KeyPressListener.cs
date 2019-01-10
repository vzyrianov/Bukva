using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Bukva
{
    class KeyPressListener
    {
        public bool Listen { get; set; }
        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private Thread keyPressListenerThread;

        public KeyPressListener()
        {
            keyPressListenerThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if(Listen)
                    {
                        ReadKeyPresses();
                        Thread.Sleep(20);
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
            }));

            keyPressListenerThread.IsBackground = true;
            keyPressListenerThread.Start();
        }

        void ReadKeyPresses()
        {
            foreach (int i in Enum.GetValues(typeof(Keys)))
            {
                if (GetAsyncKeyState(i) == -32767)
                {
                    string key;

                    if(Control.ModifierKeys.HasFlag(Keys.Shift))
                    {
                        key = Enum.GetName(typeof(Keys), i).ToUpper();
                    } 
                    else
                    {
                        key = Enum.GetName(typeof(Keys), i).ToLower();
                    }

                    OnKeyPressed(this, new KeyPressedEventArgs(key));
                }
            }
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        public string KeyPressed { get; private set; }

        public KeyPressedEventArgs(string key)
        {
            KeyPressed = key;
        }
    }
}
