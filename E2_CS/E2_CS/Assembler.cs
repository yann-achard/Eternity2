using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace E2_CS
{
    class Assembler
    {
        Piece[] rotations1;
        CompPiece[] rotations2;

        public void Solve(Problem prob)
        {
            //int size = pb.wd * pb.ht;
            rotations1 = prob.pieces.SelectMany(p => Enumerable.Range(0, 4).Select(r => p.Spined(r))).ToArray();
            int rotSize = rotations1.Length;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // a | b
            //--- ---
            // d | c

            #region 2 by 2
            int c2by2 = 0;
            List<CompPiece> rg2by2 = new List<CompPiece>();
            Dictionary<int, List<int>>[] perpiece = Enumerable.Range(0, 256).Select(p => new Dictionary<int, List<int>>()).ToArray();
            Action<int, int, CompPiece> populate = (int p, int idx, CompPiece cp) =>
            {
                Dictionary<int, List<int>> dic = perpiece[p];
                if (!dic.ContainsKey(cp.t)) dic.Add(cp.t, new List<int>());
                if (!dic.ContainsKey(cp.r)) dic.Add(cp.r, new List<int>());
                if (!dic.ContainsKey(cp.b)) dic.Add(cp.b, new List<int>());
                if (!dic.ContainsKey(cp.l)) dic.Add(cp.l, new List<int>());
                dic[cp.t].Add(idx);
                dic[cp.r].Add(idx);
                dic[cp.b].Add(idx);
                dic[cp.l].Add(idx);
            };

            for (int a = 0; a < rotSize; ++a)
            {
                int ar = rotations1[a].r;
                int ab = rotations1[a].b;
                if (ab != 0 && ar != 0)
                {
                    for (int b = 0; b < rotSize; ++b)
                    {
                        int bb = rotations1[b].b;
                        if (rotations1[b].l == ar && bb != 0
                            && ((rotations1[a].t == 0) == (rotations1[b].t == 0))
                            && (a >> 2) != (b >> 2))
                        {
                            for (int c = 0; c < rotSize; ++c)
                            {
                                int ct = rotations1[c].t;
                                int cl = rotations1[c].l;
                                if (ct == bb && cl != 0
                                    && ((rotations1[b].r == 0) == (rotations1[c].r == 0))
                                    && (a >> 2) != (c >> 2) && (c >> 2) != (b >> 2))
                                {
                                    for (int d = 0; d < rotSize; ++d)
                                    {
                                        if (rotations1[d].r == cl && rotations1[d].t == ab
                                            && ((rotations1[a].l == 0) == (rotations1[d].l == 0))
                                            && ((rotations1[d].b == 0) == (rotations1[c].b == 0))
                                            && (a >> 2) != (d >> 2) && (d >> 2) != (b >> 2) && (d >> 2) != (c >> 2))
                                        {
                                            CompPiece p = new CompPiece(
                                                rotations1[a].t << 8 | rotations1[b].t,
                                                rotations1[b].r << 8 | rotations1[c].r,
                                                rotations1[d].b << 8 | rotations1[c].b,
                                                rotations1[a].l << 8 | rotations1[d].l,
                                                new int[] {
                                                    a, b,
                                                    d, c
                                                });
                                            rg2by2.Add(p);
                                            populate(a >> 2, c2by2, p);
                                            populate(b >> 2, c2by2, p);
                                            populate(c >> 2, c2by2, p);
                                            populate(d >> 2, c2by2, p);
                                            ++c2by2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("secs: " + sw.Elapsed);
            Console.WriteLine("c2by2: " + c2by2);
            #endregion


            sw.Reset();
            sw.Start();
            float c4by4 = 0.0f;
            for (int a=0; a<c2by2; ++a)
            {
                CompPiece cpa = rg2by2[a];
                for (int pb = 0; pb < 256; ++pb)
                {
                    if (pb == cpa.comp[0] || pb == cpa.comp[1] || pb == cpa.comp[2] || pb == cpa.comp[3]) continue;
                    if (!perpiece[pb].ContainsKey(cpa.r)) continue;
                    foreach (int b in perpiece[pb][cpa.r])
                    {
                        CompPiece cpb = rg2by2[b];
                        if (a == cpb.comp[0] || a == cpb.comp[1] || a == cpb.comp[2] || a == cpb.comp[3]) continue;
                        for (int pc = 0; pc < 256; ++pc)
                        {
                            if (pc == cpa.comp[0] || pc == cpa.comp[1] || pc == cpa.comp[2] || pc == cpa.comp[3]) continue;
                            if (pc == cpb.comp[0] || pc == cpb.comp[1] || pc == cpb.comp[2] || pc == cpb.comp[3]) continue;

                        }
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("secs: " + sw.Elapsed);
            Console.WriteLine("c4by4: " + c4by4);
        }
    }
}
