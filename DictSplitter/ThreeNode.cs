using System.Collections.Generic;
using System.Linq;

namespace DictSplitter
{
    class ThreeNode
    {
        public List<ThreeNode> NextNodes = new();
        public int UNICODEnumber { get; set; } = new();
        public bool IsWord { get; set; } = false;
        public int Deep { get; set; } = 0;

        public ThreeNode this[int UNICODEnumber]
        {
            get => NextNodes.FirstOrDefault(node => node.UNICODEnumber == UNICODEnumber);
            set
            {
                if (NextNodes.FirstOrDefault(node => node.UNICODEnumber == UNICODEnumber) is null)
                {
                    NextNodes.Add(value);
                }
            }
        }

        public ThreeNode(int UNICODEnumber = 0, int deep = 0)
        {
            this.UNICODEnumber = UNICODEnumber;
            Deep = deep;
        }
    }
}
