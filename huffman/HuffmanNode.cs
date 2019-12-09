using System;
using System.Collections.Generic;

namespace huffman
{
    public class HuffmanNode
    {
        public string Symbol;
        public int Value;
        public HuffmanNode Parent;
        public HuffmanNode Left;
        public HuffmanNode Right;
        public string Path;
        public bool Leaf => Left == null && Right == null;

        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(Value)}: {Value}, {nameof(Path)}: {Path}";
        }

        public void Print(string indent = "", bool last = false)
        {

            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "\t";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }

            Console.WriteLine(this);
            var children = new List<HuffmanNode>();
            if (this.Left != null)
                children.Add(this.Left);
            if (this.Right != null)
                children.Add(this.Right);

            for (var i = 0; i < children.Count; i++)
                children[i].Print(indent, i == children.Count - 1);
        }
    }
}