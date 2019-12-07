using System;
using System.Collections.Generic;
using System.Linq;

namespace huffman
{
    public class Huffman
    {
        public static Tuple<int, string> ConstructParentNodeProps(IEnumerable<KeyValuePair<string, int>> kvp)
        {
            if(kvp is null)
                throw new ArgumentNullException($"Parameter {nameof(kvp)} cannot be null.");

            var keyValuePairs = kvp as KeyValuePair<string, int>[] ?? kvp.ToArray();
                
            return keyValuePairs.Count() > 1
                ? new Tuple<int, string>(keyValuePairs.ElementAt(0).Value + keyValuePairs.ElementAt(1).Value,
                    keyValuePairs.ElementAt(0).Key + keyValuePairs.ElementAt(1).Key)
                : new Tuple<int, string>(keyValuePairs.ElementAt(0).Value, keyValuePairs.ElementAt(0).Key);
        }
    }
}