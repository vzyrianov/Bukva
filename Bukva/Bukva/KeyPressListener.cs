using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Bukva
{
    abstract class KeyPressListener
    {
        public bool Listen { get; set; }
        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;

        protected void RaiseKeyPressedEvent(string keyPressed)
        {
            OnKeyPressed(this, new KeyPressedEventArgs(keyPressed));
        }

        public abstract void DeleteLastKeyPressed();
        public abstract void EmitBackspace();
    }


    class LowLevelKeyboardHook : KeyPressListener
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private Thread keyPressListenerThread;

        public LowLevelKeyboardHook()
        {

        }

        public override void DeleteLastKeyPressed()
        {
            SendKeys.SendWait("{BACKSPACE}");
        }

        public override void EmitBackspace()
        {
            SendKeys.SendWait("{BACKSPACE}");
        }
    }

    class KeyStateListener : KeyPressListener
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private Thread keyPressListenerThread;

        public KeyStateListener()
        {
            keyPressListenerThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if(Listen)
                    {
                        ReadKeyPresses();
                        Thread.Sleep(10);
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

                    if (key.ToUpper() == "OEMPERIOD")
                    {
                        key = ".";
                    }

                    RaiseKeyPressedEvent(key);
                }
            }
        }

        public override void DeleteLastKeyPressed()
        {
            SendKeys.SendWait("{BACKSPACE}");
        }

        public override void EmitBackspace()
        {
            SendKeys.SendWait("{BACKSPACE}");
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