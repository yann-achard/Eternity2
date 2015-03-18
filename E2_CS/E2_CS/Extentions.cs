using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	static class Extentions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			Random rng = new Random();
			T tmp;
			for (int i=2; i<list.Count; ++i) {
				int idx = rng.Next(i);
				tmp = list[idx];
				list[idx] = list[i];
				list[i] = tmp;
			}
		}

		public static void Shuffle<T>(this T[] a)
		{
			Random rng = new Random();
			T tmp;
			for (int i=2; i<a.Length; ++i) {
				int idx = rng.Next(i);
				tmp = a[idx];
				a[idx] = a[i];
				a[i] = tmp;
			}
		}
	}
}
