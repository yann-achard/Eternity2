using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class ProblemGenerator
	{

		public Problem Gen(int w, int h, int nbPat, int seed, out Board b)
		{
			Random rng = new Random(seed);
			Problem p = new Problem(w, h, nbPat);

			b = new Board(w,h,nbPat);
			HashSet<Piece> set = new HashSet<Piece>();
			--w;
			--h;
			for (int x = 0; x <= w; ++x)
			{
				for (int y = 0; y <= h; ++y)
				{
					Piece piece = new Piece();
					piece.l   = (x == 0) ? 0 : b.Get(x-1,y  ).r;
					piece.b = (y == 0) ? 0 : b.Get(x  ,y-1).t;
					for (int attempt = 0; attempt < 10; ++attempt)
					{
						piece.t   = (y == h) ? 0 : rng.Next(1, nbPat);
						piece.r = (x == w) ? 0 : rng.Next(1, nbPat);
						if (!set.Contains(piece)) break;
					}
					set.Add(piece);
					b.Set(x,y, piece);
					p.pieces.Add(piece);
				}
			}
			b.CopyToClipboard();
			return p;
		}
	}
}
