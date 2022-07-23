using System.IO;
using System.Collections.Generic;
namespace Ks2Cpp 
{
	public class Lexer 
	{
		public List<List<Token>> Tokens { get; private set;} = new ();
		public Lexer(string path)
		{
			string data = File.ReadAllText(path);
			List<Token> subtokens = new();
			
			string builder = "";
			bool buildingString = false;
			
			for (int i = 0; i < data.Length; i++)
			{
				char c = data[i];
				
				if (" \t\n:;'".Contains(c))
				{
					if (buildingString) 
					{
						if (c != '\'') { builder += c; continue; }
						else
						{
							subtokens.Add(new Token("string", '"' + builder + '"'));
							buildingString = false;
							builder = "";
							continue;
						}
					}
					
					if (!string.IsNullOrEmpty(builder))
					{
						subtokens.Add(new Token("id", builder));
						builder = "";
					}
					
					switch (c)
					{
						case '\'':
							buildingString = true;
						break;
						
						case ':':
							subtokens.Add(new Token("label", c));
						break;
						
						case ';':
							subtokens.Add(new Token("comment", c));
							
							string jbuild = "";
							for (int j = i; j < data.Length; j++)
							{
								if (data[j] != '\n') jbuild += data[j];
								else break;
							}
							subtokens.Add(new Token("comment_id", jbuild));
							subtokens.Add(new Token("newline", c));
							
							jbuild =  "";
							Tokens.Add(subtokens);
							subtokens = new();
						break;
						
						case '\n':
							subtokens.Add(new Token("newline", c));
							Tokens.Add(subtokens);
							subtokens = new();
						break;
					}
					
				}
				else
				{
					builder += c;
				}
			}
		}
	}
}