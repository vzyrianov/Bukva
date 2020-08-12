using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Bukva
{
    abstract class KeyPressListener
    {
        public bool Listen { get; set; }
        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;

        protected void RaiseKeyPressedEvent(string keyPressed)
        {
            OnKeyPressed(this, new KeyPressedEventArgs(keyPressed, false));
        }

        protected void RaiseKeyPressedEvent(string keyPressed, bool scrollLockPressed)
        {
            OnKeyPressed(this, new KeyPressedEventArgs(keyPressed, scrollLockPressed));
        }

        public abstract void DeleteLastKeyPressed();
        public abstract void EmitBackspace();
    }

    class LowLevelKeyboardHook : KeyPressListener , IDisposable
    {
        private Win32.CallBackHandler callBackHandler;
        private IntPtr hookID = IntPtr.Zero;

        private bool trap;

        private byte[] keyboardState;
        private StringBuilder converterBuffer;

        public LowLevelKeyboardHook()
        {

            callBackHandler = HookCallback;
            trap = false;
            HookKeyboard();
        }

        public void HookKeyboard()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, callBackHandler, Win32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void Dispose()
        {
            Win32.UnhookWindowsHookEx(hookID);
        }

        private string VirtualKeyToString(int virtualKeyCode)
        {
            keyboardState = new byte[256];
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
                keyboardState[(int) Keys.ShiftKey] = 0xff; 
            //else
                //keyboardState[(int) Keys.ShiftKey] = 0x0;

            converterBuffer = new StringBuilder(256);
            Win32.ToUnicode((uint)virtualKeyCode, 0, keyboardState, converterBuffer, 256, 0);
            return converterBuffer.ToString();
        }

        private bool userKey = true;
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (userKey)
                userKey = false;
            else
                return Win32.CallNextHookEx(hookID, nCode, wParam, lParam);

            if (nCode >= 0 && (wParam == (IntPtr)Win32.WM_KEYDOWN || wParam == (IntPtr)Win32.WM_SYSKEYDOWN) && Listen)
            {
                int virtualKeyCode = Marshal.ReadInt32(lParam);

                string key = VirtualKeyToString(virtualKeyCode);
                Console.WriteLine(key);

                if (key.ToUpper() == "OEMPERIOD")
                {
                    key = ".";
                }

                RaiseKeyPressedEvent(key, virtualKeyCode == 145 ? true : false);
                
                if(trap) {
                    trap = false;
                    userKey = true;
                    return (System.IntPtr)1;
                }
            }

            trap = false;
            userKey = true;
            return Win32.CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public override void DeleteLastKeyPressed()
        {
            trap = true;
        }

        public override void EmitBackspace()
        {
            Backspace();
        }

        void Backspace()
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
                    if (Listen)
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

                    if (Control.ModifierKeys.HasFlag(Keys.Shift))
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
        public bool ScrollLockPressed { get; private set; }

        public KeyPressedEventArgs(string key, bool scrollLockPressed)
        {
            KeyPressed = key;
            ScrollLockPressed = scrollLockPressed;
        }
    }
}