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
		
		public float LastDelta
		{
			get;
			private set;
		}
		
		public StatStabilizer(float satifyingSdevChangeRatio = 0.1f, uint desiredIterationCountAtStatisfaction = 100)
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

		public override void Feed(float val)
		{
			base.Feed(val);
			float newSdev = SDev;
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

		private float prevSdev;
		private float satifyingSdevRatio;
		private uint desiredIterationCountAtStatisfaction;
		private uint satisfiedInARow;
	}
}
