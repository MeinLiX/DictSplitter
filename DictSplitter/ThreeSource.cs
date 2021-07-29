using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DictSplitter
{
    class ThreeSource
    {
        public string DictinaryPath = @$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\de-dictionary.tsv";
        public ThreeNode MainNode = new();
        //public List<string> Dictionary = new();

        public void InitialTree(bool debug = true)
        {
            if (!File.Exists(DictinaryPath))
                throw new FileNotFoundException();

            if (debug)
                Console.WriteLine($"{nameof(InitialTree)}: is started...");

            List<string> Dictionary = Reader.ReadTSV(DictinaryPath);

            //skip single letters
            Parallel.ForEach(Dictionary.Where(word => word.Length > 1), (str) =>
                {
                    lock (MainNode)
                    {
                        ThreeNode currNode = MainNode;
                        foreach (byte ASCIInum in Encoding.ASCII.GetBytes(str.ToLower())) // ToLower bad idea (nouns are capitalized)
                        {
                            currNode[ASCIInum] = new(ASCIInum, currNode.Deep + 1);
                            currNode = currNode[ASCIInum];
                        }
                        currNode.IsWord = true;
                    }
                });

            if (debug)
                Console.WriteLine($"{nameof(InitialTree)}: is completed.");
        }

        public List<string> Algo(List<string> words)
        {
            List<string> res = new();

            Parallel.ForEach(words.AsParallel(), (mainWord) =>
            //words.ForEach(mainWord =>
            {
                List<SubWord> subWords = new();
                for (int i = 0; i < mainWord.Length; i++)
                {
                    SubWord subWord = AdditionAlgo(mainWord, i);
                    if (subWord is not null)
                    {
                        subWords.Add(subWord);
                        i = subWord.EndIdx - 1;
                    }
                    else if (i == 0)
                    {
                        break;
                    }
                    //perevirka
                }

                //ubraty
                if (subWords.Sum(i => i.EndIdx - i.StartIdx) != mainWord.Length)
                    subWords.Clear();

                if (subWords.Count == 0)
                {
                    res.Add(Reader.ConvertToTSV(mainWord, "->", mainWord));
                }
                else
                {
                    res.Add(Reader.ConvertToTSV(subWords.First().MainWord, "->", subWords.Select(subWord => subWord.Word).ToList()));
                }
            });
            return res;
        }

        private SubWord AdditionAlgo(string mainWord, int startIdx)
        {
            List<SubWord> subWords = new();
            ThreeNode currNode = MainNode;

            foreach (byte ASCIInum in Encoding.ASCII.GetBytes(mainWord[startIdx..]))
            {
                currNode = currNode[ASCIInum];

                if (currNode is null)
                    break;

                if (currNode.IsWord)
                    subWords.Add(new(startIdx, startIdx + currNode.Deep, mainWord));
            }

            if (subWords.Count == 0)
            {
                return null;
            }
            else
            {
                return subWords.OrderByDescending(s => s.Word.Length).First();
            }

        }
    }
}
