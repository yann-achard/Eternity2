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
			Count = 0;
			sum = 0;
			sumOfSquares = 0;
		}

		public virtual void Feed(float val)
		{
			++Count;
			sum += val;
			sumOfSquares += val*val;
		}

		public float Count
		{
			get;
			private set;
		}

		public float Avg
		{
			get { return sum/Count; }
		}

		public float SDev
		{
			get { return (float)Math.Sqrt((float)( (sumOfSquares/Count) - Avg*Avg )); }
		}

		private float sum;
		private float sumOfSquares;
	}
}
