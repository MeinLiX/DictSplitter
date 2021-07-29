using System.Collections.Generic;
using System.Linq;

namespace DictSplitter
{
    class ThreeNode
    {
        public List<ThreeNode> NextNodes = new();
        public int ASCIInumber { get; set; } = new();
        public bool IsWord { get; set; } = false;
        public int Deep { get; set; } = 0;

        public ThreeNode this[int ASCIInumber]
        {
            get => NextNodes.FirstOrDefault(node => node.ASCIInumber == ASCIInumber);

            set
            {
                if (NextNodes.FirstOrDefault(node => node.ASCIInumber == ASCIInumber) is null)
                {
                    NextNodes.Add(value);
                }
            }
        }

        public ThreeNode(int ASCIInumber = 0, int deep = 0)
        {
            this.ASCIInumber = ASCIInumber;
            Deep = deep;
        }
    }
}
