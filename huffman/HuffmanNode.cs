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
    }

    public static class HuffmanNodeEx
    {
        private class NodeInfo
        {
            public HuffmanNode Node;
            public string Text;
            public int StartPos;
            public int Size => Text.Length;
            public int EndPos { 
                get => StartPos + Size;
                set => StartPos = value - Size;
            }
            public NodeInfo Parent, Left, Right;
        }
        
        /// <summary>
        /// Prints out a graph of the tree
        /// Credits: https://stackoverflow.com/a/36496436
        /// </summary>
        /// <param name="root"></param>
        /// <param name="topMargin"></param>
        /// <param name="leftMargin"></param>
        public static void Print(this HuffmanNode root, int topMargin = 2, int leftMargin = 2)
        {
            if (root == null) return;
            var rootTop = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = root;
            for (var level = 0; next != null; level++)
            {
                var item = new NodeInfo { Node = next, Text = $"{next.Symbol}({next.Value})"};
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + 1;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.Left)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                    }
                }
                
                next = next.Left ?? next.Right;
                for (; next == null; item = item.Parent)
                {
                    Print(item, rootTop + 2 * level);
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos;
                        next = item.Parent.Node.Right;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos;
                        else
                            item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void Print(NodeInfo item, int top)
        {
            SwapColors();
            Print(item.Text, top, item.StartPos);
            SwapColors();
            if (item.Left != null)
                PrintLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
            if (item.Right != null)
                PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
        }

        private static void PrintLink(int top, string start, string end, int startPos, int endPos)
        {
            Print(start, top, startPos);
            Print("─", top, startPos + 1, endPos);
            Print(end, top, endPos);
        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }

        private static void SwapColors()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
        }
    }
}