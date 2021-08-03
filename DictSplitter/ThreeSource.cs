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

        /// <summary>
        /// A method that generates a tree based on a dictionary
        /// </summary>
        public void InitialTree()
        {
            if (!File.Exists(DictinaryPath))
                throw new FileNotFoundException();

            List<string> Dictionary = Reader.ReadTSV(DictinaryPath);

            Parallel.ForEach(
                Dictionary
                .Where(word => word.Length > 1)
                .Select(word => word.ToLower()), (str) =>
                  {
                      lock (MainNode)
                      {
                          ThreeNode currNode = MainNode;
                          foreach (byte UNICODEnum in Encoding.UTF8.GetBytes(str))
                          {
                              currNode[UNICODEnum] = new(UNICODEnum, currNode.Deep + 1);
                              currNode = currNode[UNICODEnum];
                          }
                          currNode.IsWord = true;
                      }
                  });
        }

        /// <summary>
        /// The main algorithm for splitting words into existing ones from the dictionary
        /// </summary>
        /// <param name="words">words to split</param>
        /// <returns>List in tsv strings format</returns>
        public List<string> Algo(List<string> words)
        {
            List<string> res = new();

            words.ForEach((mainWord) =>
            {
                List<SubWord> subWords = new();
                for (int i = 0; i < mainWord.Length; i++)
                {
                    SubWord subWord = SearchSubWord(mainWord, i);

                    if (subWord.EndIdx == -1)
                    {
                        subWords.Clear();
                        break;
                    }

                    subWords.Add(subWord);
                    i = subWord.EndIdx - 1;
                }
                res.Add(Reader.ConvertToTSV(
                    (subWords.Count == 0) ? mainWord : subWords.First().GetMainWord,
                    "->",
                    (subWords.Count == 0) ? mainWord : subWords.Select(subWord => subWord.GetWord()).ToList()));
            });
            return res;
        }

        /// <summary>
        /// A method for finding a subword
        /// </summary>
        /// <param name="mainWord">The word in which we look for a subword</param>
        /// <param name="startIdx">the index where the subword search begins</param>
        /// <returns>Subword.EndIdx= -1, when subword is not found.</returns>
        private SubWord SearchSubWord(string mainWord, int startIdx)
        {
            SubWord subWord = new(mainWord, startIdx);
            ThreeNode currNode = MainNode;

            foreach (byte UNICODEnum in Encoding.UTF8.GetBytes(mainWord[startIdx..]))
            {
                currNode = currNode[UNICODEnum];
                if (currNode is null)
                    break;

                if (currNode.IsWord)
                    subWord.EndIdx = startIdx + currNode.Deep;
            }
            return subWord;
        }
    }
}
