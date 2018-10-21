using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Threading;
using System.Collections;

namespace Bukva
{
    class KeyTranslator
    {
        public bool Enabled { get; set; }

        KeyboardHook keyboardHook;
        KeyPressChord keyPressHistory;
        string previouslyOutputtedKey = "";
        
        CustomLanguageConfiguration language;
        
        public KeyTranslator(CustomLanguageConfiguration languageConfig)
        {
            language = languageConfig;

            keyPressHistory = new KeyPressChord();

            keyboardHook = new KeyboardHook();
            keyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
            keyboardHook.HookKeyboard();
        }

        private void KeyboardHook_OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            KeyPress keyPress = new KeyPress();
            keyPress.key = e.KeyPressed.ToString().ToLower();
            
            keyPress.SetModifiersFromCurrentState();

            if (keyPress.isShiftDown)
                keyPress.key = keyPress.key.ToUpper();
            if (Enabled && (keyPress.key != "back") && (previouslyOutputtedKey != keyPress.key) && (keyPress.key != "none"))
            {
                keyPressHistory.AddKeyPress(keyPress);

                KeyPressChord trimmed1 = keyPressHistory.Trim(1);
                KeyPressChord trimmed2 = keyPressHistory.Trim(2);
                if (language.ContainsKeyFromChord(keyPressHistory))
                {
                    string key = language.GetKeyFromChord(keyPressHistory);
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        previouslyOutputtedKey = key;
                        Thread.Sleep(10);
                        SendKeys.SendWait("{BACKSPACE}");
                        SendKeys.SendWait("{BACKSPACE}");
                        SendKeys.SendWait(key);
                    }).Start();
                }
                else if (language.ContainsKeyFromChord(trimmed1))
                {
                    string key = language.GetKeyFromChord(trimmed1);
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        previouslyOutputtedKey = key;
                        Thread.Sleep(10);
                        SendKeys.SendWait("{BACKSPACE}");
                        SendKeys.SendWait("{BACKSPACE}");
                        SendKeys.SendWait(key);
                    }).Start();
                }
                else if (language.ContainsKeyFromChord(trimmed2))
                {
                    string key = language.GetKeyFromChord(trimmed2);
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        previouslyOutputtedKey = key;
                        Thread.Sleep(10);
                        SendKeys.SendWait("{BACKSPACE}");
                       SendKeys.SendWait(key);
                    }).Start();
                }
            }
        }
    }
}
