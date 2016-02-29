using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    struct CompPiece
    {
        public int t, r, b, l;
        public int[] comp;

        public CompPiece(int t, int r, int b, int l, int[] comp)
        {
            this.t = t;
            this.r = r;
            this.b = b;
            this.l = l;
            this.comp = comp;
        }

        public void CopyToClipboard(Piece[] r1, params CompPiece[][] rr)
        {
            int s = 2;
            for (int i = 0; i < rr.Length; ++i) s *= 2;
            int scan = s * 4;
            int[] full = new int[s * s * 4];

            Action<int[], int, int, int, int> expand = null;
            expand = (int[] abcd, int lvl, int x, int y, int l) =>
            {
                if (lvl < 0)
                {
                    int idx = y * scan + x * 4;
                    full[idx + 0] = r1[abcd[0]].t;
                    full[idx + 1] = r1[abcd[0]].r;
                    full[idx + 2] = r1[abcd[0]].b;
                    full[idx + 3] = r1[abcd[0]].l;
                    idx += 4;
                    full[idx + 0] = r1[abcd[1]].t;
                    full[idx + 1] = r1[abcd[1]].r;
                    full[idx + 2] = r1[abcd[1]].b;
                    full[idx + 3] = r1[abcd[1]].l;
                    idx += scan;
                    full[idx + 0] = r1[abcd[3]].t;
                    full[idx + 1] = r1[abcd[3]].r;
                    full[idx + 2] = r1[abcd[3]].b;
                    full[idx + 3] = r1[abcd[3]].l;
                    idx -= 4;
                    full[idx + 0] = r1[abcd[2]].t;
                    full[idx + 1] = r1[abcd[2]].r;
                    full[idx + 2] = r1[abcd[2]].b;
                    full[idx + 3] = r1[abcd[2]].l;
                }
                else
                {
                    expand(rr[lvl][abcd[0]].comp, lvl - 1, x * 2, y * 2, l);
                    expand(rr[lvl][abcd[1]].comp, lvl - 1, x * 2 + 2, y * 2, l);
                    expand(rr[lvl][abcd[2]].comp, lvl - 1, x * 2, y * 2 + 2, l);
                    expand(rr[lvl][abcd[3]].comp, lvl - 1, x * 2 + 2, y * 2 + 2, l);
                }
            };

            expand(comp, rr.Length-1, 0, 0, s*4);

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < s; ++y)
            {
                for (int x = 0; x < s; ++x)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        sb.Append(full[((y * s) + x) * 4 + i]);
                        sb.Append(" ");
                    }
                    sb.Append("     ");
                }
                sb.Append("\n");
            }
            //rg4by4.Add(p);
            System.Windows.Forms.Clipboard.SetText(sb.ToString());
        }
    }
}
