using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Problem
	{
		public int wd, ht;
		public int nbPat;
		public List<Piece> pieces = new List<Piece>();
        public Dictionary<int,int> colors = new Dictionary<int,int>();

        public void ComputeColourDistribution()
        {
            colors.Clear();
            for (int c=0; c<nbPat; ++c) colors.Add(c, 0);

            foreach (Piece p in pieces) {
                colors[p.t] += 1;
                colors[p.r] += 1;
                colors[p.b] += 1;
                colors[p.l] += 1;
            }
        }
	}
}
