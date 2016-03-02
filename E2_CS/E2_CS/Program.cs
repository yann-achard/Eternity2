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
        /*
         * Findings:
         * 
         * CycleSort: When it comes to the fewest number of swaps to transfom one shuffled string 
         * into another CycleSort is the best. Check git history for verification and implementation
         * 
         * 
         * 
        */
        [STAThread]
		static void Main(string[] args)
		{
            int seed = 1;
            Random rng = new Random(seed);

            Board b;
            Problem p = ProblemGenerator.Load("full CW.txt");
            //Problem p = ProblemGenerator.Gen(8, 8, 22, rng.Next(), out b);
            //Problem p = ProblemGenerator.Gen4by4();
            //p.pieces.Shuffle(rng);
            //p.pieces = p.pieces.ConvertAll(pi => pi.Spined(rng.Next(4)));

            Assembler ass = new Assembler();
            ass.Solve(p);
            Console.ReadKey(true);
            return;

			// Best: 16x16 over 19 colors @ Seed 57 => 4 seonds
			// Best: 16x16 over 18 colors @ Seed 4 => 10 seonds
			Percentile per = new Percentile();
			//string filename = @"../../../../TimeFirstSol_CleftBacktrackBasicOrdSeed0.txt";
			//File.WriteAllText(filename, "Measuring time to first solution using cleft backtracking with vanila ordering\n");
			//File.AppendAllText(filename, "size\tnbC\tnbAttempts\tmin\t25%%\t50%%\t75%%\tmax\n");
			int nbAttempts = 1;
			//using (StreamWriter sw = new StreamWriter(filename))
			{
				int size = 16;
				//for (int size = 4; size <= 16; ++size)
                { 
                    int nbCols = 23;
					//for (int nbCols = 23; nbCols <= 40; ++nbCols)
                    {
						per.Reset();
						Stopwatch timer = new Stopwatch();
						timer.Start();
						for (int attempt=1; attempt<=10; ++attempt)
						//while (stabilizer.CriterionMet == false)
						{
							//if (timer.Elapsed.TotalMinutes > 6 && attempt > 10) break;
                            p = ProblemGenerator.Gen(size, size, nbCols, rng.Next(), out b);
							{ 
								b.CopyToClipboard();
								b.PiecesToClefts();
								p.pieces.Shuffle(rng);
								p.pieces = p.pieces.ConvertAll(pi => pi.Spined(rng.Next(4)));
								CleftBacktrackSolver sol = new CleftBacktrackSolver();
								List<BoardSolution> solutions = new List<BoardSolution>();

								Action<double, int, Board, bool> collect = (double iter, int idx, Board board, bool isSol) => {
									if (isSol && solutions.Count == 0) {
										solutions.Add(new BoardSolution(board, iter));
									}
								};

								Stopwatch watch = new Stopwatch();
								watch.Start();
								double iterCount = sol.Solve(p, false, collect/*, b*/);
								watch.Stop();
								//Console.WriteLine("{0} found {3} in {1} iterations / {2}", sol.Name, iterCount, watch.Elapsed, solutions.Count);
								if (solutions.Count == 0) {
									b.CopyToClipboard();
								    Console.WriteLine("\b\b\b\b\bNo solution found in {0:0000} iterations", iterCount);
			                        Console.ReadKey(true);
								}
								else
								{
									if (p.IsEquivalentTo(solutions[0].board))
										;//b.CopyToClipboard(); // GOOD!
									else
										b.CopyToClipboard(); // BAD!
								}

								//StandardDev diffSdev = new StandardDev();
								//StandardDev distSdev = new StandardDev();

								//Action<double, int, Board, bool> action = (double iter, int idx, Board board, bool isSol) => {
								//	double diff = solutions.Min(s => s.board.Diff(board));
								//	double dist = solutions.Min(s => Math.Abs(iterCount - s.foundOnIteration)) / iterCount;
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

								per.Feed(watch.Elapsed.TotalSeconds);
								//Console.WriteLine(f);
								//Console.Write("\b\b\b\b\b{0:0000}", attempt);
								nbAttempts = attempt;
							}
							//++seed;
							timer.Stop();
							Console.WriteLine(timer.Elapsed.TotalMilliseconds);
							timer.Reset();
							timer.Start();
						}
						//Console.Write("\b\b\b\b\b");
						
						//File.AppendAllText(filename, string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n",
						//	size, nbCols, nbAttempts, per.GetPercentile(0), per.GetPercentile(25), per.GetMedian(), per.GetPercentile(75), per.GetPercentile(100)));
					}
				}
			}
			Console.WriteLine("\nDone");
			Console.ReadKey(true);
		}
	}
}
