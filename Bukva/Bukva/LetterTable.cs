using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bukva
{
    class LetterTable : Dictionary<string, string>
    {
        public string Filename { get; private set; }

        public LetterTable() : base()
        {
            List<string> list = GetCandidateFiles();
            Filename = list[0].Split('\\').Last();
            ReadInFromFile(list[0]);
        }

        public void ReadInFromFile(string filename)
        {
            string line;

            using (StreamReader streamReader = new StreamReader(filename))
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

        List<string> GetCandidateFiles()
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
