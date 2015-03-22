using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E2_CS
{
	class StandardDev
	{
		public StandardDev()
		{
			Reset();
		}
		
		public virtual void Reset()
		{
			tmp = 0;
			Avg = 0;
			Count = 0;
		}

		public virtual void Feed(double val)
		{
			++Count;
			double delta = val - Avg;
			Avg += delta / Count;
			tmp += delta * (val - Avg);
		}

		public virtual void Feed(IEnumerable<double> vals)
		{
			foreach (double f in vals) Feed(f);
		}

		public double Count
		{
			get;
			private set;
		}

		public double Avg
		{
			get;
			private set;
		}

		public double Var
		{
			get { return tmp/(Count-1); }
		}

		public double SDev
		{
			get { return Math.Sqrt(Var); }
		}

		double tmp;
	}
}
