namespace Bukva
{
    class FixedLengthQueue<Type>
    {
        private readonly int length;
        private int position;

        Type[] buffer;

        public FixedLengthQueue(int length)
        {
            this.length = length;
            buffer = new Type[length];
            position = 0;
        }

        public Type At(int index)
        {
            return buffer[(index + position) % length];
        }

        public void Insert(Type s)
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
            buffer = new Type[length];
            position = 0;
        }
        
        public void Clear(Type clearVar)
        {
            position = 0;

            for(int i = 0; i < length; ++i)
            {
                buffer[i] = clearVar;
            }
        }
    }
}
