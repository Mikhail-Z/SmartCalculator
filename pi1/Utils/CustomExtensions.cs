using System;
using System.Collections.Generic;
using System.Text;

namespace pi1.Utils
{
	public static class CustomExtensions
	{
		public static List<char> ToCharList(this IList<string> list)
		{
			var chars = new List<char>(list.Count);
			foreach (var symb in list)
			{
				if (symb.Length != 1)
				{
					throw new ArgumentException("Список должен состоять из однобуквенных строк");
				}
				chars.Add(symb[0]);
			}

			return chars;
		}
	}
}
