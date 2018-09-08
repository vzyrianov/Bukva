using System;
using System.Collections.Generic;
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
        private static extern IntPtr SetWindowsHookEx(int idHook, CallBackHandler lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        
        public delegate IntPtr CallBackHandler(int nCode, IntPtr wParam, IntPtr lParam);

        private CallBackHandler proc;
        private IntPtr hookID = IntPtr.Zero;

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private Dictionary<Key, bool> keysToTrap = new Dictionary<Key, bool>();

        public bool Trap { get; set; }

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

                
                //Trap current key only if trapping is enabled and the key is in the list of keys to trap
                bool result = false;
                bool trapCurrentKey = (Trap && keysToTrap.TryGetValue(key, out result)) ? result : false;   

                OnKeyPressed(this, new KeyPressedArgs(key, trapCurrentKey));

                if (trapCurrentKey)
                {
                    return (System.IntPtr)1;
                }
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hookID);
        }

        public void TrapKey(Key k)
        {
            keysToTrap[k] = true;
        }

        public void UntrapKey(Key k)
        {
            keysToTrap[k] = false;
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
