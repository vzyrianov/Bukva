using System;
using System.Collections.Generic;

namespace Bukva
{
    struct KeyPressChord 
    {
        public LinkedList<KeyPress> keyPressList;
        const int maxHistoryCount = 3;

        public void AddKeyPress(KeyPress toAdd)
        {
            if (keyPressList == null)
                keyPressList = new LinkedList<KeyPress>();

            keyPressList.AddLast(toAdd);

            if(keyPressList.Count > maxHistoryCount)
            {
                keyPressList.RemoveFirst();
            }
        }

        public KeyPressChord Trim(int amount)
        {
            KeyPressChord result = new KeyPressChord();
            result.keyPressList = new LinkedList<KeyPress> (keyPressList);

            for(int i = 0; i < amount; ++i)
            {
                if(result.keyPressList.Count > 1)
                    result.keyPressList.RemoveFirst();
            }

            return result;
        }

        public override string ToString()
        {
            string result = "";

            foreach(KeyPress kp in keyPressList)
            {
                if (kp.isShiftDown)
                {
                    result += kp.key.ToUpper();
                }
                else
                {
                    result += kp.key.ToLower();
                }
            }

            return result;
        }
    }
}
