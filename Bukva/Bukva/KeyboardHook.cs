using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        private LowLevelKeyboardProc proc;
        private IntPtr hookID = IntPtr.Zero;

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private bool trap = true;

        public KeyboardHook()
        {
            proc = HookCallback;
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
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int virtualKeyCode = Marshal.ReadInt32(lParam);
                Key key = KeyInterop.KeyFromVirtualKey(virtualKeyCode);

                OnKeyPressed(this, new KeyPressedArgs(key, trap));

                if (trap)
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
        public bool Trapped { get; private set; }

        public KeyPressedArgs(Key key, bool trapped)
        {
            KeyPressed = key;
            Trapped = trapped;
        }
    }
}
