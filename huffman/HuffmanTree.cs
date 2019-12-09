using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman
{
    /// <summary>
    /// A class designed to Huffman-encode strings.
    /// </summary>
    public class HuffmanTree
    {
        public HuffmanNode Root { get; private set; }

        /// <summary>
        /// Builds a huffman tree based on an input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The current instance of a <see cref="HuffmanTree"/>.</returns>
        /// <exception cref="InvalidOperationException">When a tree has already been built.</exception>
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
        
        /// <summary>
        /// Traverse the tree while setting the Path values of each node
        /// </summary>
        /// <param name="root"></param>
        /// <param name="recursionDepth"></param>
        /// <param name="path"></param>
        private void SetCodes(HuffmanNode root = null, int recursionDepth = 0, string path = "")
        {
            if (root == null)
            {
                if (recursionDepth == 0)
                    root = Root;
                else
                    return;
            }

            root.Path = new BitArray(path
                .Select(x => x == '1')
                .ToArray());
            
            SetCodes(root.Left, ++recursionDepth, path + "0");
            SetCodes(root.Right, ++recursionDepth, path + "1");
        }
        
        /// <summary>
        /// Recursively search a specific value in the tree
        /// </summary>
        /// <param name="value">The value to be searched for.</param>
        /// <param name="root">The root <see cref="HuffmanNode"/> from where the search begins.</param>
        /// <param name="recursionDepth">Specifies how deep we are in the call stack.
        /// It lets us decide whether it has been called for the first time or are we
        /// deeper in the call stack.
        /// </param>
        /// <returns>The corresponding node with the value or null if there isn't such node.</returns>
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
        
        /// <summary>
        /// Returns an ordered <see cref="IEnumerable"/> of all the nodes.
        /// </summary>
        /// <param name="root">The starting node to begin traversing from.</param>
        /// <param name="recursionDepth">Specifies how deep we are in the call stack.
        /// It lets us decide whether it has been called for the first time or are we
        /// deeper in the call stack.
        /// </param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Encodes a string based on the previously built tree.
        /// </summary>
        /// <param name="message">The input string to be encoded.</param>
        /// <returns>A string containing the decoded message.</returns>
        /// <exception cref="InvalidOperationException">If the tree is empty.</exception>
        public BitArray Encode(string message)
        {
            if(Root == null)
                throw new InvalidOperationException("The tree has not been built yet.");
            
            var bits = new List<bool>();
            var nodes = OrderedNodes()
                .ToArray();

            foreach (var c in message)
            {
                bits.AddRange(nodes
                    .First(x => x.Symbol == c.ToString() && x.Leaf).Path
                    .Cast<bool>());
            }
            
            return new BitArray(bits.ToArray());
        }
        
        /// <summary>
        /// Decodes a <see cref="BitArray"/> to it's corresponding message.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Decode(BitArray input)
        {
            if(Root == null)
                throw new InvalidOperationException("The tree has not been built yet.");
            
            var root = Root;
            var decoded = new StringBuilder();
            
            foreach (bool b in input)
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