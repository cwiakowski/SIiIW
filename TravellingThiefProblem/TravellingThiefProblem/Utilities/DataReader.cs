using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TravellingThiefProblem.Utilities
{
    public static class DataReader
    {
        public static List<string> FilePaths { get; private set; }

        public static void GenerateFilePaths()
        {
            FilePaths = System.IO.Directory.GetFiles(@"Data\").ToList();
        }

        public static string ReadFile(string filepath)
        {
            var content = string.Empty;
            try
            {   
                using (StreamReader sr = new StreamReader(filepath))
                {
                    content = sr.ReadToEnd();
                    content = content.Replace('.', ',');
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return content;

        }
    }
}