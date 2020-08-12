using System;
using System.Windows.Forms;

namespace Bukva
{
    class LowLevelKeyTranslator : IKeyTranslator
    {
        FixedLengthQueue<string> buffer;
        LetterTable letterTable;

        KeyPressListener keyPressListener;

        public bool Translate { get; set; }

        public LowLevelKeyTranslator(LetterTable letterTable)
        {
            buffer = new FixedLengthQueue<string>(3);
            buffer.Clear("");
            this.letterTable = letterTable;

            //keyPressListener = new KeyStateListener();
            keyPressListener = new LowLevelKeyboardHook();
            keyPressListener.Listen = false;
            keyPressListener.OnKeyPressed += OnKeyPressed;
        }

        public void Start()
        {
            keyPressListener.Listen = true;
        }

        public void Stop()
        {
            keyPressListener.Listen = false;
            buffer.Clear(""); //What if OnKeyPressed is being executed at this time?
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            string key = e.KeyPressed;

            if (e.ScrollLockPressed)
            {
                Translate = !Translate;
                return;
            }

            if (!Translate)
                return;
            

            if (Control.ModifierKeys.HasFlag(Keys.Control) || key == "none" || key == "")
                return;

            Console.WriteLine(key);

            if (key == "\b" || key == "back")
            {
                HandleUndo();
            }
            else
            {
                HandleKeyPress(key);
            }

            buffer.Insert(key);
        }

        private void HandleKeyPress(string key)
        {
            if (letterTable.ContainsKey(buffer.At(1) + buffer.At(2) + key))
            {
                if (letterTable.ContainsKey(buffer.At(2)) || letterTable.ContainsKey(buffer.At(1) + buffer.At(2)))
                {
                    keyPressListener.EmitBackspace();  //Replace previous translation with new one, if it existed
                }

                if (!letterTable.ContainsKey(buffer.At(1) + buffer.At(2)))
                    keyPressListener.EmitBackspace();

                keyPressListener.DeleteLastKeyPressed();

                SendKeys.SendWait(letterTable[buffer.At(1) + buffer.At(2) + key]);
            }
            else if (letterTable.ContainsKey(buffer.At(2) + key))
            {
                keyPressListener.DeleteLastKeyPressed();

                if (letterTable.ContainsKey(buffer.At(2)))
                    keyPressListener.EmitBackspace();  //Replace previous translation with new one, if it existed


                SendKeys.SendWait(letterTable[buffer.At(2) + key]);
            }
            else if (letterTable.ContainsKey(key))
            {
                keyPressListener.DeleteLastKeyPressed();

                SendKeys.SendWait(letterTable[key]);
            }
        }

        private void HandleUndo()
        {
            keyPressListener.EmitBackspace();
            keyPressListener.DeleteLastKeyPressed();

            if (letterTable.ContainsKey(buffer.At(0) + buffer.At(1) + buffer.At(2)))
            {
                if (letterTable.ContainsKey(buffer.At(0) + buffer.At(1)))
                {
                    SendKeys.SendWait(letterTable[buffer.At(0) + buffer.At(1)]);

                    if (letterTable.ContainsKey(buffer.At(2)))
                        SendKeys.SendWait(letterTable[buffer.At(2)]);
                    else
                        SendKeys.SendWait(buffer.At(2));
                }
                else
                {
                    if (letterTable.ContainsKey(buffer.At(0)))
                        SendKeys.SendWait(letterTable[buffer.At(0)]);
                    else
                        SendKeys.SendWait(buffer.At(0));


                    if (letterTable.ContainsKey(buffer.At(1)))
                        SendKeys.SendWait(letterTable[buffer.At(1)]);
                    else
                        SendKeys.SendWait(buffer.At(1));


                    if (letterTable.ContainsKey(buffer.At(2)))
                        SendKeys.SendWait(letterTable[buffer.At(2)]);
                    else
                        SendKeys.SendWait(buffer.At(2));
                }
            }
            else if (letterTable.ContainsKey(buffer.At(1) + buffer.At(2)))
            {
                SendKeys.SendWait(letterTable[buffer.At(1)]);
                SendKeys.SendWait(letterTable[buffer.At(2)]);
            }
        }
    }
}
