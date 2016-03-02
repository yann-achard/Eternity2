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

        public void Solve(Problem pb)
        {
            //int size = pb.wd * pb.ht;
            rotations1 = pb.pieces.SelectMany( p => Enumerable.Range(0,4).Select( r => p.Spined(r) )).ToArray();
            int rotSize = rotations1.Length;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // a | b
            //--- ---
            // d | c

            #region 2 by 2
            float c2by2 = 0;
            List<CompPiece> rg2by2 = new List<CompPiece>();
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
                                            ++c2by2;
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

            #region 4 by 4
            rotations2 = rg2by2.ToArray();
            rotSize = rotations2.Length;

            sw = new Stopwatch();
            sw.Start();

            float rotSize2 = (float)rotSize * (float)rotSize;
            float rotSize3 = (float)rotSize * rotSize2;
            float c4by4totspace = rotSize3 * (float)rotSize;
            float ic4by4 = 0;
            float c4by4 = 0;
            //List<Piece> rg4by4 = new List<Piece>();
            for (int a = 0; a < rotSize; ++a)
            {
                int ar = rotations2[a].r;
                int ab = rotations2[a].b;
                if (ab != 0 && ar != 0)
                {
                    for (int b = 0; b < rotSize; ++b)
                    {
                        int bl = rotations2[b].l;
                        int bb = rotations2[b].b;
                        if (bl == ar && bb != 0)
                        {
                            for (int c = 0; c < rotSize; ++c)
                            {
                                int ct = rotations2[c].t;
                                if (ct == bb)
                                {
                                    int cl = rotations2[c].l;
                                    for (int d = 0; d < rotSize; ++d)
                                    {
                                        ++ic4by4;
                                        if (rotations2[d].r == cl && rotations2[d].t == ab)
                                        {
                                            ++c4by4;
                                            //CompPiece p = new CompPiece(
                                            //    rotations2[a].t << 16 | rotations2[b].t,
                                            //    rotations2[b].r << 16 | rotations2[c].r,
                                            //    rotations2[d].b << 16 | rotations2[c].b,
                                            //    rotations2[a].l << 16 | rotations2[d].l,
                                            //    new int[] {a, b, d, c}
                                            //);
                                            //p.CopyToClipboard(rotations1, rotations2);
                                            if (c4by4 % 1000 == 0)
                                                Console.WriteLine("c4by4: {0}\t{1}%", c4by4, ic4by4 * 100.0f / c4by4totspace);
                                        }
                                    }
                                }
                                else ic4by4 += rotSize;
                            }
                        }
                        else ic4by4 += rotSize2;
                    }
                }
                else ic4by4 += rotSize3;
            }
            sw.Stop();
            Console.WriteLine("secs: " + sw.Elapsed);
            Console.WriteLine("c4by4: " + c4by4);
            #endregion
        }
    }
}
