using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bukva
{
    class LetterTable : Dictionary<string, string>
    {
        public string Filename { get; private set; }


        //Empty Letter table
        public LetterTable() : base()
        {

        }

        public LetterTable(string filename) : base()
        {
            Filename = filename.Split('\\').Last();
            ReadInFromFile(filename);
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

        public static List<string> GetCandidateFiles()
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
