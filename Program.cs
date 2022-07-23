using System.IO;
using System;

namespace Ks2Cpp
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args.Length < 1)
			{
            	Console.WriteLine("Usage: ks2cpp <source path> [optional: output name]\n");
				return;
			}
			if (!File.Exists(args[0]))
			{
				Console.WriteLine($"Couldn't find source file \"{args[0]}\".\n");
				return;
			}
			
			Lexer lexer = new Lexer(args[0]);
			string output = Transpiler.Transpile(lexer.Tokens);
			
			if (args.Length < 2) Console.WriteLine(output);
			else
			{
				if (File.Exists(args[1])) Console.WriteLine($"Warning: Just so you know, we're overriding file \"{args[1]}\".");
				StreamWriter sw = File.CreateText(args[1]);
				sw.Write(output);
				sw.Close();
			}
        }
    }
}
