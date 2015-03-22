using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class StatStabilizer : StandardDev
	{
		public bool CriterionMet
		{
			get;
			private set;
		}
		
		public double LastDelta
		{
			get;
			private set;
		}
		
		public StatStabilizer(double satifyingSdevChangeRatio = 0.1f, uint desiredIterationCountAtStatisfaction = 100)
		{
			this.satifyingSdevRatio = satifyingSdevChangeRatio;
			this.desiredIterationCountAtStatisfaction = desiredIterationCountAtStatisfaction;
			Reset();
		}
		
		public override void Reset()
		{
			satisfiedInARow = 0;
			CriterionMet = false;
			prevSdev = -9999.0f;
			base.Reset();
		}

		public override void Feed(double val)
		{
			base.Feed(val);
			double newSdev = SDev;
			LastDelta = Math.Abs(newSdev - prevSdev) / prevSdev;
			if (LastDelta <= satifyingSdevRatio)
			{
				++satisfiedInARow;
				if (satisfiedInARow >= desiredIterationCountAtStatisfaction)
				{
					CriterionMet = true;
				}
			}
			else
			{
				satisfiedInARow = 0;
			}
			prevSdev = newSdev;
		}

		private double prevSdev;
		private double satifyingSdevRatio;
		private uint desiredIterationCountAtStatisfaction;
		private uint satisfiedInARow;
	}
}
