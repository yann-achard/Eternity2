using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			ProblemGenerator gen = new ProblemGenerator();

			Board b;
			Problem p = gen.Gen(4, 4, 4, 0, out b);
			p.pieces.Shuffle();
			Random rng = new Random();
			p.pieces = p.pieces.ConvertAll(pi => pi.Spined(rng.Next(4)));
			Stopwatch watch = new Stopwatch();
			BacktrackSolver sol = new BacktrackSolver();
			List<Board> solutions = new List<Board>();

			watch.Start();
			uint iterCount = sol.Solve(p, solutions, false);
			watch.Stop();

			Console.WriteLine("{0} found {3} in {1} iterations / {2}", sol.Name, iterCount, watch.Elapsed, solutions.Count);
			if (solutions.Count > 0) {
				solutions[0].Shuffle();
				solutions[0].CopyToClipboard();
			}
			else {
				b.Shuffle();
				b.CopyToClipboard();
			}
			Console.ReadKey();
		}
	}
}
