using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Median
	{
		List<double> temp;

		public Median()
		{
			temp = new List<double>();
		}

		public void Feed(double val)
		{
			temp.Add(val);
		}

		public void Reset()
		{
			temp.Clear();
		}

		public double GetMedian()
		{
			temp.Sort();

			int count = temp.Count;
			if (count == 0)
			{
				throw new InvalidOperationException("Empty collection");
			}
			else if (count % 2 == 0)
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
