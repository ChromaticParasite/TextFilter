using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace TextFilter
{
	class TextFilter
	{
		// This shouldn't contain any characters used in 1337 speak.
		private string SpecialChars = " (){}._-`~|#%^*[]%^=+;:<>/";

		public string FilteredText(string str)
		{
			string originalStr = str;
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
			string newStr = Regex.Replace(str, "[^" + SpecialChars + "]", "");
			return newStr;
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
		private string CensorText(string str)
		{
			// May have to change this to a different path if you move the BadWords.txt
			string BadWordsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\BadWords.txt");
			string[] BadWords = File.ReadAllLines(BadWordsPath, Encoding.UTF8);

			str = str.ToLower();
			string CleanString = str;
			
			for (int i = 0; i < str.Length; ++i)
			{
				foreach (string badWord in BadWords)
				{
					if (CleanString.Contains(badWord))
					{
						int index = CleanString.IndexOf(badWord);
						string asterisk = new string('*', badWord.Length);

						CleanString = CleanString.Remove(index, badWord.Length);
						CleanString = CleanString.Insert(index, asterisk);
					}
				}
			}

			return CleanString;
		}

		/// <summary>
		/// Re-adds any white listed words that have been removed.
		/// </summary>
		private string CheckForWhiteListedWords(string originalStr, string str)
		{
			originalStr = TranslateLeetSpeak(RemoveSpecialChars(originalStr));
			str = str.ToLower();

			// May have to change this to a different path if you move the BadWords.txt
			string WhiteListPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\WhiteList.txt");
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
		private string CleanText(string originalStr, string str)
		{
			for (int i = 0; i < str.Length; ++i)
			{
				if (IsValidIndex(str, i))
				{
					// Are the characters different?
					if (originalStr[i] != str[i])
					{
						// Is the filtered char an asterisk?
						if (str[i] != '*')
						{
							str = str.Remove(i, 1);
							str = str.Insert(i, originalStr[i].ToString());
						}
						else
						{
							// Re-add special characters
							for (int u = 0; u < SpecialChars.Length; ++u)
							{
								if (originalStr[i] == SpecialChars[u])
								{
									str = str.Insert(i, originalStr[i].ToString());
								}
							}
						}
					}
				}
				else
				{
					str = str.Insert(i, originalStr[i].ToString());
				}
			}

			return str;
		}

		/// <summary>
		/// Checks if a particular index is within the given string.
		/// </summary>
		private bool IsValidIndex(string str, int index)
		{
			return (str.Length >= index) ? true : false;
		}
	}
}
