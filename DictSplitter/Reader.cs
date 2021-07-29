using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DictSplitter
{
    class Reader
    {
        private static readonly Type ListStringType = new List<string>().GetType();

        public static List<string> ReadTSV(string path)
        {
            List<string> wordsList = new();

            using StreamReader reader = new(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                wordsList.Add(line);
            }
            return wordsList;
        }

        public static void WriteTSV(List<string> wordsList, string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException();

            using var writer = new StreamWriter(path);
            wordsList.ForEach(obj => writer.WriteLine(obj));
        }

        public static string ConvertToTSV(params object[] param) => ConvertToTSV(false, "\t", param);

        public static string ConvertToTSV(bool toAddLineBreak, string splitter, params object[] param)
        {
            StringBuilder tsvBuilder = new();

            foreach (object i in param)
            {
                Type type = i.GetType();
                if (type == ListStringType)
                {
                    ((List<string>)i).ForEach(listItem => tsvBuilder.Append(listItem.ToString() + splitter));
                }
                else
                    tsvBuilder.Append(i.ToString() + splitter);
            }

            if (toAddLineBreak)
                tsvBuilder.Append("\r\n");

            return tsvBuilder.ToString();
        }
    }
}
