using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bukva
{
    class FixedLengthQueue
    {
        private readonly int length;
        private int position;

        string[] buffer;

        public FixedLengthQueue(int length)
        {
            this.length = length;
            buffer = new string[length];
            position = 0;
        }

        public string At(int index)
        {
            return buffer[(index + position) % length];
        }

        public void Insert(string s)
        {
            buffer[position] = s;
            position += 1;

            if(position >= length)
            {
                position = 0;
            }
        }
    }
}
