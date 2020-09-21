using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System;

namespace TextFilter
{
	class TextFilter
	{
		private string WhiteListPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\WhiteList.txt"));
		private string BadWordsPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\BadWords.txt"));

		// This shouldn't contain any characters used in 1337 speak.
		private string SpecialChars = "[*(){}._-`~|#%^%^=+;:<>/]";

		public string FilteredText(string str)
		{
			string originalStr = str;
			str = str.ToLower();
			
			str = RemoveSpecialChars(str);
			str = TranslateLeetSpeak(str);
			str = CensorText(str);
			str = CheckForWhiteListedWords(originalStr, str);
			str = CleanText(originalStr, str);
			return str;
		}

		/// <summary>
		/// Removes any special character within the string.
		/// Ex. Turn "a_s.s" into "ass"
		/// </summary>
		private string RemoveSpecialChars(string str)
		{
			return Regex.Replace(str, SpecialChars, string.Empty);
		}

		/// <summary>
		/// Translate any 1337 speak into regular English.
		/// Ex. Turn "@5$" into "ass"
		/// </summary>
		private string TranslateLeetSpeak(string str)
		{
			string i = Regex.Replace(str, "[1!]", "i");
			string e = Regex.Replace(i, "[3]", "e");
			string a = Regex.Replace(e, "[4@]", "a");
			string s = Regex.Replace(a, "[5$]", "s");
			string t = Regex.Replace(s, "[7]", "t");
			string g = Regex.Replace(t, "[9]", "g");
			string o = Regex.Replace(g, "[0]", "o");
			return o;
		}

		/// <summary>
		/// Censors a particular string.
		/// Ex. Turn "ass" into "***"
		/// </summary>
		private string CensorText(string filteredStr)
		{
			string[] BadWords = File.ReadAllLines(BadWordsPath, Encoding.UTF8);

			for (int i = 0; i < filteredStr.Length; ++i)
			{
				foreach (string badWord in BadWords)
				{
					if (filteredStr.Contains(badWord))
					{
						int index = filteredStr.IndexOf(badWord);
						string asterisk = new string('*', badWord.Length);

						filteredStr = filteredStr.Remove(index, badWord.Length);
						filteredStr = filteredStr.Insert(index, asterisk);
					}
				}
			}

			return filteredStr;
		}

		/// <summary>
		/// Re-adds any white listed words that have been removed.
		/// </summary>
		private string CheckForWhiteListedWords(string originalStr, string str)
		{
			originalStr = TranslateLeetSpeak(RemoveSpecialChars(originalStr));

			string[] WhiteList = File.ReadAllLines(WhiteListPath, Encoding.UTF8);

			for (int i = 0; i < str.Length; ++i)
			{
				foreach (string WhiteWord in WhiteList)
				{
					// Get the index of the White word.
					int WhiteWordIndex = 0;
					WhiteWordIndex = originalStr.IndexOf(WhiteWord);

					// If the White Word is in the string then re-add it
					if (WhiteWordIndex >= 0)
					{
						str = str.Remove(WhiteWordIndex, WhiteWord.Length);
						str = str.Insert(WhiteWordIndex, WhiteWord);
					}
				}
			}

			return str;
		}

		/// <summary>
		/// Cleans censored text to look more like the original text but with censorship.
		/// </summary>
		private string CleanText(string originalStr, string filteredStr)
		{
			for (int i = 0; i < originalStr.Length; ++i)
			{
				if (IsValidIndex(filteredStr, i))
				{
					// Are the characters different?
					if (originalStr[i] != filteredStr[i])
					{
						// Is the filtered char an asterisk?
						if (filteredStr[i] != '*')
						{
							filteredStr = ReplaceAt(filteredStr, i, originalStr[i]);
						}
						else
						{
							// Re-add special characters
							for (int u = 0; u < SpecialChars.Length; ++u)
							{
								if (originalStr[i] == SpecialChars[u])
								{
									filteredStr = filteredStr.Insert(i, originalStr[i].ToString());
								}
							}
						}
					}
				}
				else
				{
					filteredStr = filteredStr.Insert(i, originalStr[i].ToString());
				}
			}
			return filteredStr;
		}

		/// <summary>
		/// Checks if a particular index is within the given string.
		/// </summary>
		private static bool IsValidIndex(string str, int index)
		{
			return (index < str.Length) ? true : false;
		}

		/// <summary>
		/// Replaces a character within a given string at a particular index.
		/// </summary>
		/// <param name="inputStr">String to be updated.</param>
		/// <param name="index">Index to replace the character.</param>
		/// <param name="newChar">Replacement character.</param>
		private static string ReplaceAt(string inputStr, int index, char newChar)
		{
			if (inputStr == null)
			{
				throw new ArgumentNullException("inputStr");
			}

			char[] chars = inputStr.ToCharArray();
			chars[index] = newChar;
			return new string(chars);
		}
	}
}
