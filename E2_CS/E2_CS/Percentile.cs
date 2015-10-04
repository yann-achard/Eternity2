using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Percentile
	{
		List<double> temp;
		bool isSorted;

		public Percentile()
		{
			temp = new List<double>();
			isSorted = false;
		}

		public void Feed(double val)
		{
			temp.Add(val);
			isSorted = false;
		}

		public void Reset()
		{
			temp.Clear();
			isSorted = false;
		}

		private void EnsureSorted()
		{
			if (temp.Count == 0)
			{
				throw new InvalidOperationException("Empty collection");
			}
			if (!isSorted)
			{
				temp.Sort();
				isSorted = true;
			}
		}

		public double GetPercentile(double p)
		{
			EnsureSorted();
			int idx = (int)(p / 100.0 * (double)(temp.Count-1));
			return temp[idx];
		}

		public double GetMedian()
		{
			EnsureSorted();
			int count = temp.Count;
			if (count % 2 == 0)
			{
				// count is even, average two middle elements
				double a = temp[count / 2 - 1];
				double b = temp[count / 2];
				return (a + b) / 2;
			}
			else
			{
				// count is odd, return the middle element
				return temp[count / 2];
			}
		}
	}
}
