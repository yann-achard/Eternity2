using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	public delegate void RefAction<T>(ref T arg);

	static class Extentions
	{
		public static void Shuffle<T>(this IList<T> list, Random rng)
		{
			T tmp;
			for (int i=2; i<list.Count; ++i) {
				int idx = rng.Next(i);
				tmp = list[idx];
				list[idx] = list[i];
				list[i] = tmp;
			}
		}

		public static void Shuffle<T>(this T[] a, Random rng)
		{
			T tmp;
			for (int i=2; i<a.Length; ++i) {
				int idx = rng.Next(i);
				tmp = a[idx];
				a[idx] = a[i];
				a[i] = tmp;
			}
		}

		public static void Each<T>(this T[] a, RefAction<T> action)
		{
			for (int i=0; i<a.Length; ++i)
			{
				action(ref a[i]);
			}
		}
	}
}
