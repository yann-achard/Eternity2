using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace E2_CS
{
	class Program
	{
		static int CountInversions(int[] ar)
		{
			int nb = 0;
			for (int i=0; i<ar.Length; ++i)
			{
				for (int j = i + 1; j < ar.Length; ++j)
				{
					if (ar[i] > ar[j]) 
					{
						++nb;
						break;
					}
				}
			}
			return nb;
		}

		static int CycleSort(int[] ar)
		{
			int len = ar.Length;
			int[] a = new int[len];
			Array.Copy(ar, a, len);
			int iter = 0;
			for (int i = 0; i < len; ++i)
			{
				while (a[i] != i)
				{
					int v = a[i];
					a[i] = a[v];
					a[v] = v;
					++iter;
				}
			}
			return iter;
		}

		static int SwapBFS(int[] ar)
		{
			int len = ar.Length;
			Queue<int[]> q = new Queue<int[]>();
			Queue<int> q2 = new Queue<int>();
			q.Enqueue(ar);
			q2.Enqueue(0);
			while (q.Count > 0)
			{
				int[] a = q.Dequeue();
				int d = q2.Dequeue();
				bool sorted = true;
				for (int i = 0; i < len; ++i)
				{
					if (a[i] != i)
					{
						sorted = false;
						int[] a2 = new int[len];
						Array.Copy(a, a2, len);
						a2[a2[i]] = a[i];
						a2[i] = a[a[i]];
						q.Enqueue(a2);
						q2.Enqueue(d+1);
					}
				}
				if (sorted)
				{
					return d;
				}
			}
			return -1;
		}

		static void TestMinSwapCountTheory()
		{
			int seed = 4;
			int len = 8;
			int nbIter = 1000;
			Random rng = new Random(seed);
			int[] ar = new int[len];
			for (int i=0; i<len; ++i) ar[i] = i;
			for (int it = 0; it < nbIter; ++it) { 
				ar.Shuffle(rng);
				int nbInv = CycleSort(ar);
				int shortestPath = SwapBFS(ar);
				if (nbInv != shortestPath)
				{
					Debug.Assert(false);
				}
			}
		}


		[STAThread]
		static void Main(string[] args)
		{
			int seed = 0;
			Random rng = new Random(seed);
			double stabilityGoal = 0.01f;
			uint stableInARow = 100;
			StatStabilizer stabilizer = new StatStabilizer(stabilityGoal, stableInARow);
			string filename = @"../../../../NbItersCleftBacktrackSeed0.txt";
			File.WriteAllText(filename, "Measuing avg nb of iterations to first solution using cleft swapping\n");
			File.AppendAllText(filename, "s\tnbC\tavg  \tsdev   \truns\tmiliseconds\n");
			//using (StreamWriter sw = new StreamWriter(filename))
			{
				int size = 2; 
				//for (int size = 4; size <= 7; ++size)
                { 
                    int nbCols = size+2;
					//for (int nbCols = 3; nbCols <= 7; ++nbCols)
                    {
						stabilizer.Reset();
						Stopwatch watch = new Stopwatch();
						watch.Start();
						for (int attempt=0; attempt<10000; ++attempt)
						//while (stabilizer.CriterionMet == false)
						{
							ProblemGenerator gen = new ProblemGenerator();

							Board b;
							Problem p = gen.Gen(size, size, nbCols, rng.Next(), out b);
							//for (;;)
							{ 
								p.pieces.Shuffle(rng);
								p.pieces = p.pieces.ConvertAll(pi => pi.Spined(rng.Next(4)));
								CleftBacktrackSolver sol = new CleftBacktrackSolver();
								List<BoardSolution> solutions = new List<BoardSolution>();

								Action<double, int, Board, bool> collect = (double iter, int idx, Board board, bool isSol) => {
									if (isSol) {
										solutions.Add(new BoardSolution(board, iter));
									}
								};

								double iterCount = sol.Solve(p, false, collect);
								//Console.WriteLine("{0} found {3} in {1} iterations / {2}", sol.Name, iterCount, watch.Elapsed, solutions.Count);
								if (solutions.Count == 0) {
									b.CopyToClipboard();
								    Console.WriteLine("\b\b\b\b\bNo solution found in {0:0000} iterations", iterCount);
			                        Console.ReadKey(true);
								}

								//StandardDev diffSdev = new StandardDev();
								//StandardDev distSdev = new StandardDev();

								//Action<double, int, Board, bool> action = (double iter, int idx, Board board, bool isSol) => {
								//	double diff = solutions.MinBy(s => s.board.Diff(board));
								//	double dist = solutions.MinBy(s => Math.Abs(iterCount - s.foundOnIteration)) / iterCount;
								//	diffSdev.Feed(diff);
								//	distSdev.Feed(dist);
								//};

								//sol.Solve(p, true, action);

								//double avgdiff = diffSdev.Avg;
								//double avgdist = distSdev.Avg;
								//double sdevdiff = diffSdev.SDev;
								//double sdevdist = distSdev.SDev;
								//double corr = (diffSdev.Var * distSdev.Var) / (sdevdiff * sdevdist);

								//string f = string.Format("s{0} c{1} i{2:0.} avgdiff:{3} avgdist:{4} sdevdiff:{5} sdevdist:{6} corr:{7}",
								//	size, nbCols, iterCount, avgdiff, avgdist, sdevdiff, sdevdist, corr);

								stabilizer.Feed(iterCount);
								//Console.WriteLine(f);
								File.AppendAllText(filename, string.Format("{0}\t{1}\t{2}\n", size, nbCols, iterCount));
								Console.Write("\b\b\b\b\b{0:0000}", stabilizer.Count);
								//Console.WriteLine("{0:0.00E-00} \t {1:0.00E-00} \t {2}", stabilizer.Avg, stabilizer.SDev, stabilizer.LastDelta);
								//ConsoleKeyInfo k = Console.ReadKey(true);
								//if (k.Key != ConsoleKey.Enter) break;
							}
							++seed;
						} // while (stabilizer.CriterionMet == false)
						watch.Stop();
						double milisec = (double)watch.ElapsedMilliseconds;
						Console.Write("\b\b\b\b\b");
						Console.WriteLine("{0} {1} {2:0.00E-00}\t{3:0.00E-00}\t{4}\t{5:0.0}", size, nbCols, stabilizer.Avg, stabilizer.SDev, stabilizer.Count, milisec);
						//File.AppendAllText(filename, string.Format("{0}\t{1}\t{2:0.0}\t{3:0.0}\t{4:0.0}\t{5:0.0}\n", size, nbCols, stabilizer.Avg, stabilizer.SDev, stabilizer.Count, milisec));
					}
				}
			}
			Console.ReadKey(true);
		}
	}
}
