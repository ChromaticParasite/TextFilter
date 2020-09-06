using System;
using TextFilter;

namespace TextFilter
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;

			// [Input, Expected Output]
			string[,] testCases = new string[,]
			{
				{"ass", "***" },
				{"@55", "***" },
				{"pass", "pass" },
				{"p@ss", "p@ss" },
				{"The ass is mad.", "The *** is mad." },
				{"The @55 is mad.", "The *** is mad." },
				{"The @55 i5 m@d.", "The *** i5 m@d." },
				{"The ass is an ass.", "The *** is an ***." },
				{"The @$$ i5 an ass.", "The *** i5 an ***." },
				{"The @$$ i5 an @$$.", "The *** i5 an ***." },
				{"Can you pass?", "Can you pass?" },
				{"Can you p@s5?", "Can you p@s5?" },
				{"Can you pass the ass?", "Can you pass the ***?" },
				{"Can you p@s5 the @s$?", "Can you p@s5 the ***?" }
			};

			TextFilter TF = new TextFilter();

			for (int i = 0; i < testCases.GetLength(0); i++)
			{
				string OriginalText = testCases[i, 0];
				string FilteredText = TF.FilteredText(OriginalText);
				string ExpectedText = testCases[i, 1];

				Console.WriteLine("Case: " + i);
				Console.WriteLine("    Original: " + OriginalText);
				Console.WriteLine("    Filtered: " + FilteredText);
				Console.WriteLine("    Expected: " + ExpectedText);
				if (FilteredText.Equals(ExpectedText))
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Result: Pass");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Result: Fail");
				}
				Console.ForegroundColor = ConsoleColor.White;
				//Console.WriteLine();
			}

		}
	}
}
