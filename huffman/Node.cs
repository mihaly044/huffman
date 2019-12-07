using System;

namespace huffman
{
    public class Node
    {
        public int Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public string Symbol { get; set; }
        public Node Parent { get; set; }
        public bool IsLeaf { get; set; }
        public string Path { get; set; }
    }

    public static class NodeEx
    {
        public static void SetCodes(this Node node, string path = "")
        {
            if (node == null)
                return;
            
            node.Path = path;
            node.IsLeaf = node.Left == null && node.Right == null;
            

            SetCodes(node.Left, path + "0");
            SetCodes(node.Right, path + "1");
        }

        public static void Traverse(this Node node)
        {
            if (node == null)
                return;

            if(node.Symbol.Length == 1)
                Console.WriteLine($"{node.Symbol} [=] {node.Path}");
            
            Traverse(node.Left);
            Traverse(node.Right);
        }
    }
}