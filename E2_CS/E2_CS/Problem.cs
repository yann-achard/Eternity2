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
        public int[] colors;

        public Problem(int width, int height, int nbPatterns)
        {
            wd = width;
            ht = height;
            nbPat = nbPatterns;
            colors = new int[nbPat];
        }

        public void ComputeColourDistribution()
        {
            colors.FillWith(0);
            foreach (Piece p in pieces) {
                colors[p.t] += 1;
                colors[p.r] += 1;
                colors[p.b] += 1;
                colors[p.l] += 1;
            }
        }
	}
}
