using System.Text;

namespace DictSplitter
{
    class SubWord
    {
        public int StartIdx { get; set; }
        public int EndIdx { get; set; }
        private byte[] MainWord { get; }
        public string GetMainWord => GetWord(subWord: false);

        public string GetWord(bool subWord = true)
        {
            string word;

            if (StartIdx is not -1 && EndIdx is not -1 && subWord)
                word = Encoding.UTF8.GetString(MainWord[StartIdx..EndIdx]);
            else
                word = Encoding.UTF8.GetString(MainWord);

            return word;
        }

        public SubWord(byte[] mainWord, int startIdx = -1, int endIdx = -1)
        {
            StartIdx = startIdx;
            EndIdx = endIdx;
            MainWord = mainWord;
        }

        public SubWord(string mainWord, int startIdx = -1, int endIdx = -1) : this(Encoding.UTF8.GetBytes(mainWord), startIdx, endIdx)
        { }
    }
}
