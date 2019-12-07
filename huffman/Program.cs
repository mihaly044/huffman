using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            const string input = "aaaaabbbbbbbbccccccccccccddddddddddddddddddddeeeef";
            
            // Count the the occurrences of each letter in the input
            var dict = new Dictionary<string, int>();
            foreach (var key in input.Select(ch => ch.ToString().ToLower()))
            {
                if (dict.Keys.Contains(key))
                    dict[key]++;
                else
                    dict[key] = 1;
            }
            
            string Kvp2Str(IEnumerable<KeyValuePair<string, int>> kvp)
            {
                var sb = new StringBuilder();
                sb.Append($"{kvp.ElementAt(0).Key} {kvp.ElementAt(0).Value}");
                
                if(kvp.Count() > 1)
                    sb.Append($" - {kvp.ElementAt(1).Key} {kvp.ElementAt(1).Value}");

                return sb.ToString();
            }

            Node parent = null;
            while (dict.Count > 1)
            {
                var mins = dict.OrderBy(x => x.Value).Take(2).ToList();
                var (parentValue, parentSymbol) = Huffman.ConstructParentNodeProps(mins);
                
                Console.WriteLine(Kvp2Str(mins) + $"\t{parentSymbol} {parentValue}");

                if (parent == null)
                {
                    parent = new Node
                    {
                        Value = parentValue,
                        Symbol = parentSymbol
                    };

                    
                    parent.Left = new Node
                    {
                        Value = mins[0].Value,
                        Symbol = mins[0].Key,
                        Parent = parent
                    };
                    
                    parent.Right = new Node
                    {
                        Value = mins[1].Value,
                        Symbol = mins[1].Key,
                        Parent = parent
                    };
                }
                else 
                {
                    var newParent = new Node
                    {
                        Right = parent,
                        Symbol = parentSymbol,
                        Value = parentValue
                    };

                    newParent.Left = new Node
                    {
                        Parent = newParent,
                        Symbol = mins[0].Key,
                        Value = mins[0].Value
                    };

                    parent.Parent = newParent;
                    parent = newParent;
                }
                
                dict.Remove(mins[0].Key);
                if (mins.Count <= 1) 
                    continue;
                dict.Remove(mins[1].Key);
                dict.Add(parentSymbol, parentValue);
            }
            
            Console.WriteLine("\nTraversing:");
            
            parent.SetCodes();
            parent.Traverse();
        }
    }
}