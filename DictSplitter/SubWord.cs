
namespace DictSplitter
{
    class SubWord
    {
        public int StartIdx { get; }
        public int EndIdx { get; }
        public string MainWord { get; }
        public string Word => StartIdx != -1 && EndIdx != -1 ? MainWord[StartIdx..EndIdx] : MainWord;

        public SubWord(int startIdx, int endIdx, string mainWord)
        {
            StartIdx = startIdx;
            EndIdx = endIdx;
            MainWord = mainWord;
        }
    }
}
