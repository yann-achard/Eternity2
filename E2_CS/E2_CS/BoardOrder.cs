using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{

	interface BoardOrder
	{
		Piece Get(int idx);
		void Set(int idx, Piece p);
		int GetSide(Side side, int idx);
	}
}
