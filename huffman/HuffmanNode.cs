using System;
using System.Collections;
using System.Collections.Generic;

namespace huffman
{
    /// <summary>
    /// The nodes that build up the <see cref="HuffmanTree"/>.
    /// </summary>
    public class HuffmanNode
    {
        public string Symbol;
        public int Value;
        public HuffmanNode Parent;
        public HuffmanNode Left;
        public HuffmanNode Right;
        public BitArray Path;
        public bool Leaf => Left == null && Right == null;

        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(Value)}: {Value}, {nameof(Path)}: {Path.ToBinaryString()}";
        }

        /// <summary>
        /// Print this node and all of its children to the console.
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="last"></param>
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
            if (Left != null)
                children.Add(Left);
            if (Right != null)
                children.Add(Right);

            for (var i = 0; i < children.Count; i++)
                children[i].Print(indent, i == children.Count - 1);
        }
    }
}