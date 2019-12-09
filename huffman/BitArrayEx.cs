using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman
{
    public static class BitArrayEx
    {
        /// <summary>
        /// Converts a <see cref="BitArray"/> to its string representation
        /// </summary>
        /// <param name="arr">The input array</param>
        /// <returns>A string representation of the input array</returns>
        public static string ToBinaryString(this BitArray arr)
        {
            var sb = new StringBuilder();
            foreach (bool b in arr)
            {
                sb.Append(b ? "1" : "0");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Constructs a <see cref="BitArray"/> from an input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The newly created BitArray.</returns>
        public static BitArray FromBinaryString(string input)
        {
            var bits = new List<bool>();
            foreach (var c in input)
            {
                bits.Append(input == "1");
            }

            return new BitArray(bits.ToArray());
        }
    }
}