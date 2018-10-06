using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Bukva
{
    class KeyboardHook : IDisposable
    {
        private Win32.CallBackHandler callBackHandler;
        private IntPtr hookID = IntPtr.Zero;

        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;

        private HashSet<Key> keysToTrap = new HashSet<Key>();

        public bool Trap { get; set; }

        public KeyboardHook()
        {
            callBackHandler = HookCallback;
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

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)Win32.WM_KEYDOWN || wParam == (IntPtr)Win32.WM_SYSKEYDOWN))
            {
                int virtualKeyCode = Marshal.ReadInt32(lParam);
                Key pressedKey = KeyInterop.KeyFromVirtualKey(virtualKeyCode);

                bool trapCurrentKey = Trap && keysToTrap.Contains(pressedKey);

                OnKeyPressed(this, new KeyPressedEventArgs(pressedKey, trapCurrentKey));

                if (trapCurrentKey)
                {
                    return (System.IntPtr)1;
                }
            }

            return Win32.CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public void TrapKey(Key k)
        {
            keysToTrap.Add(k);
        }

        public void UntrapKey(Key k)
        {
            keysToTrap.Remove(k);
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }
        public bool Trapped { get; private set; }

        public KeyPressedEventArgs(Key key, bool trapped)
        {
            KeyPressed = key;
            Trapped = trapped;
        }
    }
}
