using System.Collections.Generic;
using System.IO;

namespace Bukva
{
    class LetterTable : Dictionary<string, string>
    {
        public LetterTable() : base()
        {

        }

        public void ReadInFromFile()
        {
            string line;

            using (StreamReader streamReader = new StreamReader("lang.buk"))
            {
                while (!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();

                    if (line.Contains(","))
                    {
                        var tokens = line.Split(',');
                        Add(tokens[0], tokens[1]);
                    }
                }
            }
        }

        public List<string> GetCandidateFiles()
        {
            List<string> languageFiles = new List<string>();
            string[] files = Directory.GetFiles(".");

            foreach(string file in files)
            {
                if (file.EndsWith(".buk"))
                    languageFiles.Add(file);
            }
            
            return languageFiles;
        }
    }
}
