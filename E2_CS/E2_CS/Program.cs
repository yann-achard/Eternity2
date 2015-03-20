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
		[STAThread]
		static void Main(string[] args)
		{
			int seed = 3;
			Random rng = new Random(seed);
			float stabilityGoal = 1000.01f;
			uint stableInARow = 1;
			StatStabilizer stabilizer = new StatStabilizer(stabilityGoal, stableInARow);
			string filename = @"../../../../Timing2.txt";
			File.WriteAllText(filename, "Measuing avg nb of iterations to first solution using snail backtracker\n");
			File.AppendAllText(filename, "size\tnbCols\tavg\tsdev\truns\tmiliseconds\n");
			//using (StreamWriter sw = new StreamWriter(filename))
			{
				int size = 7; 
				//for (int size = 7; size <= 7; ++size)
                { 
                    int nbCols = 7;
					//for (int nbCols = 6; nbCols <= 6; ++nbCols)
                    {
						stabilizer.Reset();
						Stopwatch watch = new Stopwatch();
						watch.Start();
						while (stabilizer.CriterionMet == false)
						{
							ProblemGenerator gen = new ProblemGenerator();

							Board b;
							Problem p = gen.Gen(size, size, nbCols, rng.Next(), out b);
							//for (;;)
							{ 
								//p.pieces.Shuffle(rng);
								//p.pieces = p.pieces.ConvertAll(pi => pi.Spined(rng.Next(4)));
								BacktrackSolver sol = new BacktrackSolver();
								List<BoardSolution> solutions = new List<BoardSolution>();

								float iterCount = sol.Solve(p, solutions, false);

								//Console.WriteLine("{0} found {3} in {1} iterations / {2}", sol.Name, iterCount, watch.Elapsed, solutions.Count);
								if (solutions.Count > 0) {
								    Console.WriteLine("Solution found in {0:0000} iterations", iterCount);
								//	float min = solutions.Min(s => s.foundOnIteration);
								//	float avg = solutions.Aggregate<BoardSolution,float>(0, (float v, BoardSolution s) => v + s.foundOnIteration / (float)solutions.Count);
								//	float max = solutions.Max(s => s.foundOnIteration);
								//	Console.WriteLine("imin:{0:0.E-00} iavg:{1:0.E-00} imax:{2:0.E-00}", min, avg, max);
								//	solutions[0].board.Shuffle(new Random());
									solutions[0].board.CopyToClipboard();
								}
								else {
								    Console.WriteLine("No solution found in {0:0000} iterations", iterCount);
			                        Console.ReadKey(true);
								//	b.Shuffle(rng);
									b.CopyToClipboard();
								}
								stabilizer.Feed(iterCount);
								Console.Write("\b\b\b\b\b{0:0000}", stabilizer.Count);
								//Console.WriteLine("{0:0.00E-00} \t {1:0.00E-00} \t {2}", stabilizer.Avg, stabilizer.SDev, stabilizer.LastDelta);
								//ConsoleKeyInfo k = Console.ReadKey(true);
								//if (k.Key != ConsoleKey.Enter) break;
							}
							++seed;
						} // while (stabilizer.CriterionMet == false)
						watch.Stop();
						float milisec = (float)watch.ElapsedMilliseconds;
						Console.Write("\b\b\b\b\b");
						Console.WriteLine("{0} {1} {2:0.00E-00}\t{3:0.00E-00}\t{4}\t{5:0.0}", size, nbCols, stabilizer.Avg, stabilizer.SDev, stabilizer.Count, milisec);
						File.AppendAllText(filename, string.Format("{0}\t{1}\t{2:0.0}\t{3:0.0}\t{4:0.0}\t{5:0.0}\n", size, nbCols, stabilizer.Avg, stabilizer.SDev, stabilizer.Count, milisec));
					}
				}
			}
			//Console.ReadKey(true);
		}
	}
}
