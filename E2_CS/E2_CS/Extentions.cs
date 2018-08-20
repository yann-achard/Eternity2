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
        public static void Enqueue<TKey,TVal>(this Dictionary<TKey,List<TVal>> dic, TKey key, TVal v)
        {
            List<TVal> list;
            if (dic.TryGetValue(key, out list))
            {
                list.Add(v);
            }
            else
            {
                list = new List<TVal>(1);
                list.Add(v);
                dic.Add(key, list);
            }
        }

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
				{
					throw new InvalidOperationException("Sequence was empty");
				}
				TSource min = sourceIterator.Current;
				TKey minKey = selector(min);
				while (sourceIterator.MoveNext())
				{
					TSource candidate = sourceIterator.Current;
					TKey candidateProjected = selector(candidate);
					if (comparer.Compare(candidateProjected, minKey) < 0)
					{
						min = candidate;
						minKey = candidateProjected;
					}
				}
				return min;
			}
		}

        public static IEnumerable<int> Repeat(this Random rng, int max, int count)
        {
            for (int i=0; i<count; ++i) yield return rng.Next(max);
        }

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

		public static void FillWith<T>(this T[] a, T val)
		{
			int d0 = a.GetLength(0);
			for (int i = 0; i < d0; ++i) a[i] = val;
		}

		public static void FillNWith<T>(this T[] a, int n, T val)
		{
			for (int i = 0; i < n; ++i) a[i] = val;
		}

        public static void FillWith<T>(this T[] a, Func<T> init)
		{
            int d0 = a.GetLength(0);
            for (int i = 0; i < d0; ++i) a[i] = init();
		}

        public static void FillWith<T>(this T[] a, Func<int,T> init)
		{
            int d0 = a.GetLength(0);
            for (int i = 0; i < d0; ++i) a[i] = init(i);
		}

        public static void FillWith<T>(this T[,,,] a, T val)
		{
            int d0 = a.GetLength(0);
            int d1 = a.GetLength(1);
            int d2 = a.GetLength(2);
            int d3 = a.GetLength(3);
            for (int i = 0; i < d0; ++i)
                for (int j = 0; j < d1; ++j)
                    for (int k = 0; k < d2; ++k)
                        for (int l = 0; l < d3; ++l)
                            a[i,j,k,l] = val;
		}
	}
}
