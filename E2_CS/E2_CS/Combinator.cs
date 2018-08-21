using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    class Combinator
    {
        struct Corner
        {
            public UInt64 a;
            public UInt64 b;
            public Corner(int _a, int _b)
            {
                a = (ulong)_a;
                b = (ulong)_b;
            }
            public Corner(UInt64 _a, UInt64 _b)
            {
                a = _a;
                b = _b;
            }
        }

        struct Mask
        {
            UInt64 a;
            UInt64 b;
            UInt64 c;
            UInt64 d;

            public Mask(UInt64 _a, UInt64 _b, UInt64 _c, UInt64 _d)
            {
                a = _a;
                b = _b;
                c = _c;
                d = _d;
            }
            public Mask(int p)
            {
                a = 0;
                b = 0;
                c = 0;
                d = 0;
                if (p < 64)
                    a |= (1ul << p);
                else if (p < 128)
                    b |= (1ul << (p-64));
                else if (p < 192)
                    c |= (1ul << (p - 128));
                else
                    d |= (1ul << (p - 192));
            }
            public bool Overlaps(ref Mask rhs)
            {
                return ((rhs.a & a) | (rhs.b & b) | (rhs.c & c) | (rhs.d & d)) != 0;
            }
            public Mask(ref Mask m1, ref Mask m2)
            {
                a = m1.a | m2.a;
                b = m1.b | m2.b;
                c = m1.c | m2.c;
                d = m1.d | m2.d;
            }
        }

        class ChunkEqualityComparer : IEqualityComparer<Chunk>
        {
            public bool Equals(Chunk c1, Chunk c2)
            {
                return c1.mask.Equals(c2.mask) && (
                    (c1.a == c2.a && c1.b == c2.b && c1.c == c2.c && c1.d == c2.d) ||
                    (c1.a == c2.b && c1.c == c2.b && c1.d == c2.c && c1.a == c2.d) ||
                    (c1.a == c2.c && c1.d == c2.b && c1.a == c2.c && c1.b == c2.d) ||
                    (c1.a == c2.d && c1.a == c2.b && c1.b == c2.c && c1.c == c2.d));
            }

            public int GetHashCode(Chunk c)
            {
                return c.hash;
            }
        }

        class Chunk
        {
            public Mask mask;
            public UInt64 a;
            public UInt64 b;
            public UInt64 c;
            public UInt64 d;
            public Chunk tl;
            public Chunk tr;
            public Chunk br;
            public Chunk bl;
            public byte rtl;
            public byte rtr;
            public byte rbr;
            public byte rbl;
            public int hash;

            public Chunk(
                ref Mask _mask,
                UInt64 _a, UInt64 _b, UInt64 _c, UInt64 _d,
                Chunk _tl, Chunk _tr, Chunk _br, Chunk _bl,
                byte _rtl, byte _rtr, byte _rbr, byte _rbl)
            {
                tl = _tl;
                tr = _tr;
                br = _br;
                bl = _bl;
                rtl = _rtl;
                rtr = _rtr;
                rbr = _rbr;
                rbl = _rbl;
                mask = _mask;
                a = _a;
                b = _b;
                c = _c;
                d = _d;
                hash = mask.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode() ^ c.GetHashCode() ^ d.GetHashCode();

                //if (_a < _b)
                //{
                //    if (_a < _c)
                //    {
                //        if (_a < _d)
                //        {
                //            a = _a;
                //            b = _b;
                //            c = _c;
                //            d = _d;
                //        }
                //        else
                //        {
                //            a = _d;
                //            b = _a;
                //            c = _b;
                //            d = _c;
                //        }
                //    }
                //    else
                //    {
                //        if (_c < _d)
                //        {
                //            a = _c;
                //            b = _d;
                //            c = _a;
                //            d = _b;
                //        }
                //        else
                //        {
                //            a = _d;
                //            b = _a;
                //            c = _b;
                //            d = _c;
                //        }
                //    }
                //}
                //else
                //{
                //    if (_b < _c)
                //    {
                //        if (_b < _d)
                //        {
                //            a = _b;
                //            b = _c;
                //            c = _d;
                //            d = _a;
                //        }
                //        else
                //        {
                //            a = _d;
                //            b = _a;
                //            c = _b;
                //            d = _c;
                //        }
                //    }
                //    else
                //    {
                //        if (_c < _d)
                //        {
                //            a = _c;
                //            b = _d;
                //            c = _a;
                //            d = _b;
                //        }
                //        else
                //        {
                //            a = _d;
                //            b = _a;
                //            c = _b;
                //            d = _c;
                //        }
                //    }
                //}
            }

            
            public void CopyToClipboard(int s)
            {
                Board board = new Board(s, s, 22);
                this.CopyToBoard(0, 0, s, board);
                board.CopyToClipboard();
            }
            private void CopyToBoard(int x, int y, int s, Board board, int rot = 0)
            {
                rot = rot % 4;
                if (s == 1)
                {
                    switch (rot)
                    {
                        case 0: board.Set(x, y, (int)a, (int)b, (int)c, (int)d); break;
                        case 1: board.Set(x, y, (int)b, (int)c, (int)d, (int)a); break;
                        case 2: board.Set(x, y, (int)c, (int)d, (int)a, (int)b); break;
                        case 3: board.Set(x, y, (int)d, (int)a, (int)b, (int)c); break;
                    }
                    
                } else
                {
                    int i = s / 2;
                    switch (rot)
                    {
                        case 0:
                            tl.CopyToBoard(x, y + i, i, board, rtl);
                            tr.CopyToBoard(x + i, y + i, i, board, rtr);
                            br.CopyToBoard(x + i, y, i, board, rbr);
                            bl.CopyToBoard(x, y, i, board, rbl);
                            break;
                        case 1:
                            tr.CopyToBoard(x, y + i, i, board, rtr + rot);
                            br.CopyToBoard(x + i, y + i, i, board, rbr + rot);
                            bl.CopyToBoard(x + i, y, i, board, rbl + rot);
                            tl.CopyToBoard(x, y, i, board, rtl + rot);
                            break;
                        case 2:
                            br.CopyToBoard(x, y + i, i, board, rbr + rot);
                            bl.CopyToBoard(x + i, y + i, i, board, rbl + rot);
                            tl.CopyToBoard(x + i, y, i, board, rtl + rot);
                            tr.CopyToBoard(x, y, i, board, rtr + rot);
                            break;
                        case 3:
                            bl.CopyToBoard(x, y + i, i, board, rbl + rot);
                            tl.CopyToBoard(x + i, y + i, i, board, rtl + rot);
                            tr.CopyToBoard(x + i, y, i, board, rtr + rot);
                            br.CopyToBoard(x, y, i, board, rbr + rot);
                            break;
                    }
                }
            }
        }

        struct Match
        {
            public Chunk chunk;
            public UInt64 c;
            public UInt64 d;
            public byte rot;

            public Match(Chunk _chunk, UInt64 _c, UInt64 _d, byte _rot)
            {
                chunk = _chunk;
                rot = _rot;
                c = _c;
                d = _d;
            }
        }

        delegate ulong Shifter(ulong u, ulong w);

        public void Solve(Problem prob)
        {
            Dictionary<Corner, List<Match>> dic1 = new Dictionary<Corner, List<Match>>(1024);
            Dictionary<Corner, List<Match>> dic2 = new Dictionary<Corner, List<Match>>(1024);
            HashSet<Chunk> uniques = new HashSet<Chunk>(new ChunkEqualityComparer());

            // Populate the dictionary 1 with the pieces
            int piece_num = 0;
            foreach (Piece p in prob.pieces)
            {
                Mask mask = new Mask(piece_num++);
                Chunk chunk = new Chunk(ref mask, (ulong)p.t, (ulong)p.r, (ulong)p.b, (ulong)p.l, null, null, null, null, 0, 0, 0, 0);

                if (p.t != 0 && p.r != 0)
                    dic1.Enqueue(new Corner(p.t,p.r), new Match(chunk, (ulong)p.b, (ulong)p.l, 0));
                if (p.r != 0 && p.b != 0)
                    dic1.Enqueue(new Corner(p.r,p.b), new Match(chunk, (ulong)p.l, (ulong)p.t, 1));
                if (p.b != 0 && p.l != 0)
                    dic1.Enqueue(new Corner(p.b,p.l), new Match(chunk, (ulong)p.t, (ulong)p.r, 2));
                if (p.l != 0 && p.t != 0)
                    dic1.Enqueue(new Corner(p.l,p.t), new Match(chunk, (ulong)p.r, (ulong)p.b, 3));
            }

            int factor = 1;
            while (factor < prob.ht)
            {
                int shift = 16 * factor;
                Shifter sh = (ulong u, ulong w) => (u << shift) | w;
                factor *= 2;

                foreach (KeyValuePair<Corner, List<Match>> kv1 in dic1)
                {
                    foreach (Match match1 in kv1.Value)
                    {
                        Chunk chunk1 = match1.chunk;
                        Mask m1 = chunk1.mask;
                        ulong a1 = kv1.Key.a;
                        ulong b1 = kv1.Key.b;
                        ulong c1 = match1.c;
                        ulong d1 = match1.d;
                        byte rot1 = (byte)((match1.rot + 3) % 4);

                        foreach (KeyValuePair<Corner, List<Match>> kv2 in dic1)
                        {
                            foreach (Match match2 in kv2.Value)
                            {
                                Chunk chunk2 = match2.chunk;
                                Mask m2 = chunk2.mask;
                                if (m1.Overlaps(ref m2)) continue;
                                ulong a2 = kv2.Key.a;
                                ulong b2 = kv2.Key.b;
                                ulong c2 = match2.c;
                                ulong d2 = match2.d;
                                byte rot2 = (byte)((match2.rot + 1) % 4);

                                Corner b2a1 = new Corner(b2, a1);
                                Corner b1a2 = new Corner(b1, a2);

                                List<Match> list3;
                                List<Match> list4;
                                if (dic1.TryGetValue(b2a1, out list3) && dic1.TryGetValue(b1a2, out list4))
                                {
                                    Mask m12 = new Mask(ref m1, ref m2);
                                    foreach (Match match3 in list3)
                                    {
                                        Chunk chunk3 = match3.chunk;
                                        Mask m3 = chunk3.mask;
                                        ulong c3 = match3.c;
                                        ulong d3 = match3.d;
                                        if (m12.Overlaps(ref m3)) continue;
                                        // No partial border
                                        if (((d1 == 0) != (c3 == 0)) || ((d3 == 0) != (c2 == 0))) continue;
                                        ulong t = sh(d1, c3);
                                        ulong r = sh(d3, c2);
                                        byte rot3 = (byte)((match3.rot + 2) % 4);

                                        Mask m123 = new Mask(ref m12, ref m3);
                                        foreach (Match match4 in list4)
                                        {
                                            Chunk chunk4 = match4.chunk;
                                            Mask m4 = chunk4.mask;
                                            if (m123.Overlaps(ref m4)) continue;
                                            ulong c4 = match4.c;
                                            ulong d4 = match4.d;
                                            // No partial border
                                            if (((d4 == 0) != (c1 == 0)) || ((d2 == 0) != (c4 == 0))) continue;
                                            ulong b = sh(c4, d2);
                                            ulong l = sh(c1, d4);

                                            Mask m1234 = new Mask(ref m123, ref m4);

                                            Chunk chunk = new Chunk(ref m1234, t, r, b, l, chunk1, chunk3, chunk2, chunk4, rot1, rot3, rot2, match4.rot);
                                            if (uniques.Add(chunk))
                                            {
                                                if (t != 0 && r != 0)
                                                    dic2.Enqueue(new Corner(t, r), new Match(chunk, b, l, 0));
                                                if (r != 0 && b != 0)
                                                    dic2.Enqueue(new Corner(r, b), new Match(chunk, l, t, 1));
                                                if (b != 0 && l != 0)
                                                    dic2.Enqueue(new Corner(b, l), new Match(chunk, t, r, 2));
                                                if (l != 0 && t != 0)
                                                    dic2.Enqueue(new Corner(l, t), new Match(chunk, r, b, 3));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                dic1 = dic2;
                dic2 = new Dictionary<Corner, List<Match>>(1024);
                if (factor < prob.ht)
                    uniques = new HashSet<Chunk>(new ChunkEqualityComparer());
                else
                    foreach (Chunk c in uniques)
                    {
                        c.CopyToClipboard(factor);
                    }
            }
        }
    }
}
