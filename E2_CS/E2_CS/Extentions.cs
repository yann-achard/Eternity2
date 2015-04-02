﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	public delegate void RefAction<T>(ref T arg);

	static class Extentions
	{
		public static Out MinBy<In,Out>(this IEnumerable<In> e, Func<In,Out> f)
		{
			return e.Select( v => f(v)).Min();
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
                        for (int l = 0; l < d2; ++l)
                            a[i,j,k,l] = val;
		}
        
	}
}
