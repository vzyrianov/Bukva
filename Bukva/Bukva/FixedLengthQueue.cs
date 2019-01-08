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

        public void Clear()
        {
            for(int i = 0; i < length; ++i)
            {
                buffer[i] = "";
            }
        }
    }
}
