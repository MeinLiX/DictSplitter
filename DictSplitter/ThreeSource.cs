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
                    else
                    {
                        subWords.Clear();
                        break;
                    }
                }

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
            SubWord subWord = new(mainWord, startIdx);
            ThreeNode currNode = MainNode;

            foreach (byte ASCIInum in Encoding.ASCII.GetBytes(mainWord[startIdx..]))
            {
                currNode = currNode[ASCIInum];

                if (currNode is null)
                    break;

                if (currNode.IsWord)
                {
                    subWord.EndIdx = startIdx + currNode.Deep;
                }
            }

            if (subWord.EndIdx == -1)
            {
                return null;
            }
            else
            {
                return subWord;
            }

        }
    }
}
