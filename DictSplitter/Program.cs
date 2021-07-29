using System;
using System.Collections.Generic;
using System.IO;

namespace DictSplitter
{
    class Program
    {
        static void Main(string[] args) 
        {
            if (args.Length != 2)
                throw new ArgumentException();

            if (!File.Exists(args[0]) && !File.Exists(args[0]))
                throw new FileNotFoundException();

            ThreeSource threeSRC = new();
            threeSRC.InitialTree();

            List<string> InputWords = Reader.ReadTSV(args[0]);
            List<string> ResultSplit = threeSRC.Algo(InputWords);
            Reader.WriteTSV(ResultSplit, args[1]);
        }
    }
}
