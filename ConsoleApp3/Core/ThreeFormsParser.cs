using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class ThreeFormsParser
    {
        public static List<(string, string, string)> ParseFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"File {filename} is not exist");

            List<(string, string, string)> rezultList = new List<(string, string, string)>();
            string[] rows = File.ReadAllLines(filename);

            foreach (string row in rows)
            {
                Regex wordPattern = new Regex(@"[\w/]+");
                MatchCollection matches = wordPattern.Matches(row);
                if (matches.Count == 3)
                {
                    rezultList.Add((matches[0].Value.ToLower(), matches[1].Value.ToLower(), matches[2].Value.ToLower()));
                }
                else
                {
                    continue;
                }
            }

            if (rezultList.Count == 0)
                throw new ArgumentException($"File {filename} has unsupported inner structure");

            return rezultList;
        }
    }
}
