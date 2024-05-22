using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace App.Scripts.External.Localisation.Converters
{
    public sealed class CsvConverter : IConverter
    {
        public string[,] ConvertFileToGrid(string csvText)
        {
            string[] lines = csvText.Split('\n');

            int width = lines.Select(SplitCsvLine).Aggregate(0, (current, row) => Mathf.Max(current, row.Length));
            string[,] outputGrid = new string[width, lines.Length + 1];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] row = SplitCsvLine(lines[i]);

                for (int j = 0; j < row.Length; j++)
                {
                    outputGrid[j, i] = row[j];
                    outputGrid[j, i] = outputGrid[j, i].Replace("\"\"", "\"");
                }
            }

            return outputGrid;
        }

        private string[] SplitCsvLine(string line)
        {
            return (from Match m in Regex.Matches
            (
                line,
                @"(((?<x>(?=[,;\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,;\r\n]+));?)",
                RegexOptions.ExplicitCapture
            ) select m.Groups[1].Value).ToArray();
        }
    }
}