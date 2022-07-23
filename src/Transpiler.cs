using System;
using System.Collections.Generic;
namespace Ks2Cpp
{
	public static class Transpiler
	{
		public static string Transpile(List<List<Token>> tokens)
		{
			string final = "// Usage: g++ -std=c++17 <file>\n";
			final += "#include <algorithm>\n#include <iostream>\n#include <string>\n#include <stack>\n#include <any>\n\nusing std::string;\n\n";	
			final += "// do NOT use namespace reserved::, its created for karmelscript interpreter compatibility reasons\n";
			final += "namespace reserved {std::stack<void*> label_stack; void label_stack_push(void* label_name) {label_stack.emplace(label_name);} void* label_stack_pop() {void* label = label_stack.top(); label_stack.pop(); return label;} std::stack<std::any> variable_stack; template <typename T> void stack_pop(T& var) {var = std::any_cast<T>(variable_stack.top()); variable_stack.pop();} template <typename T> void stack_push(T var) {variable_stack.emplace(var);} void flip(int &val) {val *= -1;} void flip(std::string &val) {std::reverse(val.begin(), val.end());}}\n\n";
			final += "\nint main()\n{\n";
			var subtArray = tokens.ToArray();
			
			uint specialJumpCounter = 0;
			foreach (var subt in subtArray)
			{
				try 
				{
					final += subt[0].Value switch {
					 	"new" => $"{subt[2].Value} {subt[1].Value};\n",
						"cpy" or
						"set" => $"{subt[1].Value} = {subt[2].Value};\n",
						"put" or
						"add" => $"{subt[1].Value} += {subt[2].Value};\n",
						"mul" => $"{subt[1].Value} *= {subt[2].Value};\n",
						"div" => $"{subt[1].Value} /= {subt[2].Value};\n",
						"mod" => $"{subt[1].Value} %= {subt[2].Value};\n",
						"sub" => $"{subt[1].Value} -= {subt[2].Value};\n",
						"len" => $"{subt[1].Value} = {subt[2].Value}.length();\n",
						"cut" => $"{subt[1].Value} = {subt[1].Value}.substr({subt[2].Value}, {subt[3].Value});\n",
						"psh" => $"reserved::stack_push({subt[1].Value});\n",
						"pop" => $"reserved::stack_pop({subt[1].Value});\n",
						"flp" => $"reserved::flip({subt[1].Value});\n",
						"out" => $"std::cout << {subt[1].Value};\n",
						"out!"=> $"std::cout << {subt[1].Value} << std::endl;\n",
						"jmp" => $"goto nr_{subt[1].Value};\n",
						':' => $"\nnr_{subt[1].Value}:\n\n",
						';' => $"\n// {subt[1].Value};\n",
						"siz" => $"if ({subt[1].Value} != 0) \n",
						"siz!" => $"if ({subt[1].Value} == 0) \n",
						"sin" => $"if ({subt[1].Value} >= 0) \n",
						"sin!" => $"if ({subt[1].Value} <= -1) \n",
						"jmp!" => $"{{reserved::label_stack_push(&&r_jmp_{specialJumpCounter}); goto nr_{subt[1].Value};}}\nr_jmp_{specialJumpCounter++}:\n",
						"end" => $"if (!reserved::label_stack.empty()) goto *reserved::label_stack_pop();\n",
						"end!" => $"return 0;\n",
						_ => ""
					};
				}
				catch (Exception) {/* throw that shit out */}
			}
			
			return final + "return 0;\n}";
		}
	}
}