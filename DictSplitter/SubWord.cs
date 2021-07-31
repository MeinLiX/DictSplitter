
namespace DictSplitter
{
    class SubWord
    {
        public int StartIdx { get; set; }
        public int EndIdx { get; set; }
        public string MainWord { get; }
        public string Word => StartIdx != -1 && EndIdx != -1 ? MainWord[StartIdx..EndIdx] : MainWord;

        public SubWord(string mainWord, int startIdx = -1, int endIdx = -1)
        {
            StartIdx = startIdx;
            EndIdx = endIdx;
            MainWord = mainWord;
        }

    }
}
