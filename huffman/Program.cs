using System;

namespace huffman
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args[0] == string.Empty)
            {
                Console.WriteLine("Please specify an input parameter.");
                return;
            }
            
            var orig = args[0];
            
            var hf = new HuffmanTree()
                .Build(orig);
            
            var encoded = hf.Encode(orig);
            var decoded = hf.Decode(encoded);
            
            Console.WriteLine($"Original message: {orig}");
            Console.WriteLine($"Encoded message: {encoded.ToBinaryString()}");
            Console.WriteLine($"Decoded message: {decoded}");

            hf.Root.Print();
        }
    }
}