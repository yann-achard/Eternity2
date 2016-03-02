using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class ProblemGenerator
	{
		static public Problem Gen(int w, int h, int nbPat, int seed, out Board b)
		{
			Random rng = new Random(seed);
			Problem p = new Problem(w, h, nbPat);

			b = new Board(w,h,nbPat);
			HashSet<int> set = new HashSet<int>();
			--w;
			--h;
			for (int x = 0; x <= w; ++x)
			{
				for (int y = 0; y <= h; ++y)
				{
					Piece piece = new Piece();
					piece.l   = (x == 0) ? 0 : b.Get(x-1,y  ).r;
					piece.b = (y == 0) ? 0 : b.Get(x  ,y-1).t;
					for (int attempt = 0; attempt < 200; ++attempt)
					{
						piece.t   = (y == h) ? 0 : rng.Next(1, nbPat);
						piece.r = (x == w) ? 0 : rng.Next(1, nbPat);
                        if (!set.Contains(piece.ToMinIntNoUnknowns())) break;
                        if (attempt == 199)
                        {
                            System.Diagnostics.Debug.WriteLine("Some pieces are not unique");
                            Console.WriteLine("Some pieces are not unique");
                        }
					}
                    set.Add(piece.ToMinIntNoUnknowns());
					b.Set(x,y, piece);
					p.pieces.Add(piece);
				}
			}
			b.CopyToClipboard();
			return p;
		}

        static public Problem Load(string file)
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            int w = 1;
            while (w*w < lines.Length) ++w;
            if (w * w > lines.Length) throw new InvalidProgramException("Loaded file does not represent a square board");
		    
            int nbPat = 0;
            List<Piece> pieces = new List<Piece>();
            foreach (string line in lines)
            {
                string[] v = line.Split(' ', ',', '\t');
                Piece p = new Piece(int.Parse(v[1]), int.Parse(v[2]), int.Parse(v[3]), int.Parse(v[4]));
                nbPat = Math.Max(nbPat, p.t);
                nbPat = Math.Max(nbPat, p.r);
                nbPat = Math.Max(nbPat, p.b);
                nbPat = Math.Max(nbPat, p.l);
                pieces.Add(p);
            }

            Problem pb = new Problem(w, w, nbPat+1);
            pb.pieces = pieces;
            return pb;
        }

        static public Problem Gen2by2()
        {
            //  a  1|1  b
            //  4  / \  2
            //-----   -----
            //  4  \ /  2
            //  d  3|3  c

            Problem pb = new Problem(2, 2, 5);
            pb.pieces.Add(new Piece(0, 1, 4, 0));
            pb.pieces.Add(new Piece(0, 0, 2, 1));
            pb.pieces.Add(new Piece(2, 0, 0, 3));
            pb.pieces.Add(new Piece(4, 3, 0, 0));
            Board b = new Board(pb.wd, pb.ht, pb.nbPat);
            for (int i = 0; i < pb.pieces.Count; ++i) b.Set(i, pb.pieces[i]);
            b.CopyToClipboard();
            return pb;
        }

        static public Problem Gen4by4()
        {
            //     1|1     2|2     3|3     
            //  4  / \  5  / \  6  / \  7
            //-----   -----   -----   -----
            //  4  \ /  5  \ /  6  \ /  7
            //     8|8     9|9    10|10    
            // 11  / \ 12  / \ 13  / \ 14
            //-----   -----   -----   -----
            // 11  \ / 12  \ / 13  \ / 14
            //    15|15   16|16   17|17    
            // 18  / \ 19  / \ 20  / \ 21
            //-----   -----   -----   -----
            // 18  \ /  19 \ / 20  \ / 21
            //    22|22   23|23   24|24    

            Problem pb = new Problem(4, 4, 25);
            pb.pieces.Add(new Piece(0, 1, 4, 0));
            pb.pieces.Add(new Piece(0, 2, 5, 1));
            pb.pieces.Add(new Piece(0, 3, 6, 2));
            pb.pieces.Add(new Piece(0, 0, 7, 3));
            pb.pieces.Add(new Piece(4, 8, 11, 0));
            pb.pieces.Add(new Piece(5, 9, 12, 8));
            pb.pieces.Add(new Piece(6, 10, 13, 9));
            pb.pieces.Add(new Piece(7, 0, 14, 10));
            pb.pieces.Add(new Piece(11, 15, 18, 0));
            pb.pieces.Add(new Piece(12, 16, 19, 15));
            pb.pieces.Add(new Piece(13, 17, 20, 16));
            pb.pieces.Add(new Piece(14, 0, 21, 17));
            pb.pieces.Add(new Piece(18, 22, 0, 0));
            pb.pieces.Add(new Piece(19, 23, 0, 22));
            pb.pieces.Add(new Piece(20, 24, 0, 23));
            pb.pieces.Add(new Piece(21, 0, 0, 24));
            Board b = new Board(pb.wd, pb.ht, pb.nbPat);
            for (int i = 0; i < pb.pieces.Count; ++i) b.Set(i, pb.pieces[i]);
            b.CopyToClipboard();
            return pb;
        }
    }
}
