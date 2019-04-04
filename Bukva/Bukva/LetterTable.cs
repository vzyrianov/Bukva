using System.Collections.Generic;
using System.IO;

namespace Bukva
{
    class LetterTable : Dictionary<string, string>
    {
        public LetterTable() : base()
        {

        }

        void ReadInFromFile()
        {
            string line;

            using (StreamReader streamReader = new StreamReader("lang.buk"))
            {
                line = streamReader.ReadLine();
                var tokens = line.Split(',');
                Add(tokens[0], tokens[1]);
            }
        }
    }
}
