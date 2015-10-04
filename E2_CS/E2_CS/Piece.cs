using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	struct Piece
	{
		public int t, r, b, l;

		public Piece(int t, int r, int b, int l)
		//public Piece(int t=0, int r=0, int b=0, int l=0)
		{
			this.t = t;
			this.r = r;
			this.b = b;
			this.l = l;
		}

		public bool EquivalentTo(Piece p)
		{
			return
				(t==p.t && r==p.r && b==p.b && l==p.l) ||
				(r==p.t && b==p.r && l==p.b && t==p.l) ||
				(b==p.t && l==p.r && t==p.b && r==p.l) ||
				(l==p.t && t==p.r && r==p.b && b==p.l);
		}

		public bool EquivalentModuloUknownsTo(Piece p)
		{
			return
				((t==p.t || t==-1 || p.t==-1) && (r==p.r || r==-1 || p.r==-1) && (b==p.b || b==-1 || p.b==-1) && (l==p.l || l==-1 || p.l==-1)) ||
				((r==p.t || r==-1 || p.t==-1) && (b==p.r || b==-1 || p.r==-1) && (l==p.b || l==-1 || p.b==-1) && (t==p.l || t==-1 || p.l==-1)) ||
				((b==p.t || b==-1 || p.t==-1) && (l==p.r || l==-1 || p.r==-1) && (t==p.b || t==-1 || p.b==-1) && (r==p.l || r==-1 || p.l==-1)) ||
				((l==p.t || l==-1 || p.t==-1) && (t==p.r || t==-1 || p.r==-1) && (r==p.b || r==-1 || p.b==-1) && (b==p.l || b==-1 || p.l==-1));
		}

		public int ToMinInt()
		{
			int[] tmp = {t, r, b, l};
			Array.Sort(tmp);
			return (((((tmp[0]<<8) | tmp[1])<<8) | tmp[2])<<8) | tmp[3];
		}

		public void Set(int t, int r, int b, int l)
		{
            this.t = t;
            this.r = r;
            this.b = b;
            this.l = l;
		}

        public void Set(Side side, int col)
        {
            switch (side)
            {
                case Side.Top: t = col; break;
                case Side.Right: r = col; break;
                case Side.Bottom: b = col; break;
                case Side.Left: l = col; break;
            }
        }

        public int Get(Side side)
        {
            switch (side)
            {
                case Side.Top: return t;
                case Side.Right: return r;
                case Side.Bottom: return b;
                case Side.Left: return l;
                default: return -1;
            }
        }

		public int Diff(ref Piece p)
		{
			return
				(t != p.t ? 1 : 0) + 
				(r != p.r ? 1 : 0) + 
				(b != p.b ? 1 : 0) + 
				(l != p.l ? 1 : 0)
			;
		}

		public Piece Spined(int nb)
		{
			int[] tmp = {t, r, b, l};
			return new Piece(tmp[(nb+0)%4], tmp[(nb+1)%4], tmp[(nb+2)%4], tmp[(nb+3)%4]);
		}

		public override string ToString()
		{
			return (t==-1?0:t) + " " + (r==-1?0:r) + " " + (b==-1?0:b) + " " + (l==-1?0:l);
		}
	}

    struct SpunPiece
    {
        public SpunPiece(Piece p, int index)
        {
            piece = p;
            pieceIndex = index;
        }

        public Piece piece;
        public int pieceIndex;
    }
}
