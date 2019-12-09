using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman
{
    public class HuffmanTree
    {
        public HuffmanNode Root { get; private set; }

        public HuffmanTree Build(string input)
        {
            if (Root != null)
                throw new InvalidOperationException("The tree has been already built.");
            
            var dict = new Dictionary<string, int>();
            foreach (var key in input
                .Select(c => c.ToString()))
            {
                if (dict.Keys.Contains(key))
                    dict[key]++;
                else
                    dict[key] = 1;
            }

            var nodes = dict
                .Select(keyValuePair => new HuffmanNode
                {
                    Symbol = keyValuePair.Key,
                    Value = keyValuePair.Value
                })
                .ToList();
            
            while (nodes.Count >= 2)
            {
                var elems = nodes
                    .OrderBy(n => n.Value)
                    .Take(2)
                    .ToList();

                var parent = new HuffmanNode
                {
                    Value = elems[0].Value + elems[1].Value,
                    Symbol = elems[0].Symbol + elems[1].Symbol,
                    Left = elems[0],
                    Right = elems[1]
                };

                parent.Left.Parent = parent;
                parent.Right.Parent = parent;
                
                nodes.Add(parent);
                nodes.Remove(elems[1]);
                nodes.Remove(elems[0]);
            }

            Root = nodes.FirstOrDefault();
            SetCodes();
            return this;
        }
        
        private void SetCodes(HuffmanNode root = null, int recursionDepth = 0, string path = "")
        {
            if (root == null)
            {
                if (recursionDepth == 0)
                    root = Root;
                else
                    return;
            }

            root.Path = path;
            
            SetCodes(root.Left, ++recursionDepth, path + "0");
            SetCodes(root.Right, ++recursionDepth, path + "1");
        }
        
        public HuffmanNode Search(int value, HuffmanNode root = null, int recursionDepth = 0)
        {
            if (root == null)
            {
                if (recursionDepth == 0)
                    root = Root;
                else
                    return null;
            }

            if (root.Value == value)
            {
                return root;
            }

            recursionDepth++;
            return Search(value, root.Left) ?? Search(value, root.Right, recursionDepth);
        }
        
        public IEnumerable<HuffmanNode> OrderedNodes(HuffmanNode root = null, int recursionDepth = 0)
        {
            if (root == null)
            {
                if (recursionDepth == 0)
                    root = Root;
                else
                    yield break;
            }

            yield return root;
            recursionDepth++;

            foreach (var node in OrderedNodes(root.Left, recursionDepth))
            {
                yield return node;
            }
            
            foreach (var node in OrderedNodes(root.Right, recursionDepth))
            {
                yield return node;
            }
        }

        public string Encode(string message)
        {
            var encoded = new StringBuilder();
            var nodes = OrderedNodes()
                .ToArray();

            foreach (var c in message)
            {
                encoded.Append(nodes
                    .First(x => x.Symbol == c.ToString() && x.Leaf).Path);
            }

            return encoded.ToString();
        }

        public string Decode(string message)
        {
            var root = Root;
            var decoded = new StringBuilder();
            
            foreach (var b in message.Select(x => x == '1'))
            {
                root = b ? root.Right : root.Left;

                if (!root.Leaf)
                    continue;
                
                decoded.Append(root.Symbol);
                root = Root;
            }

            return decoded.ToString();
        }
    }
}