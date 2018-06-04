using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace Bukva
{
    class KeyboardHook : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        public bool CaptureKeys { get; set; }

        private LowLevelKeyboardProc proc;
        private IntPtr hookID = IntPtr.Zero;

        public KeyboardHook()
        {
            proc = HookCallback;
            CaptureKeys = true;
        }

        public void HookKeyboard()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                Key key = KeyInterop.KeyFromVirtualKey(vkCode);

                bool letKeyThrough = true;

                if ((key.ToString().ToLower() == "oemquotes") || (key.ToString().ToLower() == "oemperiod"))
                    letKeyThrough = false;
                else if ((key.ToString().Length == 1) && ((key.ToString().ToLower()[0] >= 'a') && (key.ToString().ToLower()[0] <= 'z')))
                    letKeyThrough = false;


                if (OnKeyPressed != null) { OnKeyPressed(this, new KeyPressedArgs(key)); }

                if (!letKeyThrough && CaptureKeys)
                    return (System.IntPtr)1;
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hookID);
        }
    }

    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }

        public KeyPressedArgs(Key key)
        {
            KeyPressed = key;
        }
    }
}
