//#define TIMING

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace E2_CS
{
    class ContentionSolver
    {
		int nbKidMax;
		int nbKid;
		// We assign indices to kids
		int[] kidId;
		int[] layer;
		int[] firstInterest; // last
		bool[] fed;
		bool[] visited;
		int[] q;

		int nbToyMax;
		int nbToy;
		// We assign indices to toys
		int[] toyId;
		int[] owner;

		// Edges
		int[] nextInterest; // prev: index of alternative hunger or -1
		int[] interestTarget; // head: which toy is targeted by that hunger

		public ContentionSolver(int _nbKidMax, int _nbToyMax)
		{
			nbKidMax = _nbKidMax;
			layer = new int[nbKidMax];
			firstInterest = new int[nbKidMax];
			q = new int[nbKidMax];
			kidId = new int[nbKidMax];
			fed = new bool[nbKidMax];
			visited = new bool[nbKidMax];

			nbToyMax = _nbToyMax;
			toyId = new int[nbToyMax];
			owner = new int[nbToyMax];
		}


#if TIMING
		static string filename = @"../../../../SolverTimes_GoodOrder_Size6_Seed0.txt";
		static ContentionSolver()
		{
			File.WriteAllText(filename, "nKids\tnToys\tnHung\tmsTime\n");
		}
#endif

		public bool Solve<Person,Toy>(IDictionary<int, HashSet<int>> hungers, int _nbToys, IDictionary<Person,Toy> solution = null)
		{
#if TIMING
			int nbKids = hungers.Count;
			int nbToys = _nbToys;
			int nbHungers = 0;
			Stopwatch timer = new Stopwatch();
			foreach (var kv in hungers)
				nbHungers += kv.Value.Count;
			timer.Reset();
			timer.Start();
#endif
			bool res = false;
			for (int i=0; i<1000; ++i)
				res = DoSolve(hungers, _nbToys, solution);
			
#if TIMING
			timer.Stop();
			File.AppendAllText(filename,  string.Format("{0}\t{1}\t{2}\t{3}\n", nbKid, nbToy, nbHungers, timer.ElapsedMilliseconds));
#endif
			return res;
		}

		public bool DoSolve<Person,Toy>(IDictionary<int, HashSet<int>> hungers, int _nbToys, IDictionary<Person,Toy> solution = null)
		{
			int nbInterests = 0;
			nbKid = 0;
			toyId.FillNWith(_nbToys, -1);
			nbToy = 0;
			foreach (var kv in hungers) {
				fed[nbKid] = false;
				firstInterest[nbKid] = -1;
				kidId[kv.Key] = nbKid++;
				foreach (int toy in kv.Value) {
					if (toyId[toy] == -1) {
						owner[nbToy] = -1;
						toyId[toy] = nbToy++;
					}
				}
				nbInterests += kv.Value.Count;
			}

			if (nextInterest == null || nextInterest.Length < nbInterests) {
				nextInterest = new int[nbInterests];
				interestTarget = new int[nbInterests];
			}
			int iEdge = 0;
			foreach (var kv in hungers) {
				int kid = kidId[kv.Key];
				foreach (int toy in kv.Value) {
					interestTarget[iEdge] = toyId[toy];
					nextInterest[iEdge] = firstInterest[kid];
					firstInterest[kid] = iEdge++;
				}
			}


			int nbMatchesLeft = nbKid;
			while (nbMatchesLeft > 0) {
				Bfs();
				visited.FillNWith(nbKid, false);
				int batch = 0;
				for (int i=0; i<nbKid; ++i) {
					if (!fed[i] && Dfs(i)) ++batch;
				}
				if (batch == 0) return false;
				nbMatchesLeft -= batch;
			}
			return true;
		}

		private bool Dfs(int kid)
		{
			visited[kid] = true;
			for (int interest=firstInterest[kid]; interest>=0;  interest=nextInterest[interest]) {
				int toy = interestTarget[interest];
				int takenBy = owner[toy];
				if (takenBy<0 || (!visited[takenBy] && layer[takenBy] == layer[kid]+1 && Dfs(takenBy))) {
					owner[toy] = kid;
					fed[kid] = true;
					return true;
				}
			}
			return false;
		}

		private void Bfs()
		{
			layer.FillNWith(nbKid, -1);
			int qsize = 0;
			for (int i=0; i<nbKid; ++i) {
				if (!fed[i]) {
					q[qsize++] = i;
					layer[i] = 0;
				}
			}
			for (int i=0; i<qsize; ++i) {
				int kid = q[i];
				for (int interest=firstInterest[kid]; interest>=0;  interest=nextInterest[interest]) {
					int alreadyTakenBy = owner[interestTarget[interest]];
					if (alreadyTakenBy >= 0 && layer[alreadyTakenBy] < 0) {
						layer[alreadyTakenBy] = layer[kid] + 1;
						q[qsize++] = alreadyTakenBy;
					}
				}
			}
		}

        public static bool SolveOld<Person,Toy>(IDictionary<Person, HashSet<Toy>> needs, int nbToys, IDictionary<Person,Toy> solution = null)
        {
			bool save = false;

            var nbPeopleLeft = needs.Count;
            if (nbPeopleLeft > nbToys) return false;
            var toyIndices = new Dictionary<Toy, int>();
            var perToyInterests = new HashSet<Person>[nbToys];

            int nbToysLeft = 0;
            foreach (KeyValuePair<Person, HashSet<Toy>> kv in needs)
            {
				foreach (Toy c in kv.Value)
				{
					int index;
					if (toyIndices.TryGetValue(c, out index) == false) {
						index = nbToysLeft++;
						toyIndices.Add(c, index);
						perToyInterests[index] = new HashSet<Person>();
					}
					perToyInterests[index].Add(kv.Key);
				}
            }
            if (nbPeopleLeft > nbToysLeft) return false;

            var sortedIndices = Enumerable.Range(0,nbToysLeft).ToArray();
            //Array.Sort(sortedIndices, Comparer<int>.Create( (a,b) => perToyInterests[a].Count.CompareTo(perToyInterests[b].Count) ));
			var sortedReverseIndices = new int[nbToysLeft];
			for (int i=0; i<nbToysLeft; ++i) sortedReverseIndices[sortedIndices[i]] = i;

			if (save)
			{
				string filename = @"../../../../graph.txt";
				File.WriteAllText(filename, "graph {\n");
				foreach (KeyValuePair<Person, HashSet<Toy>> kv in needs)
				{
					foreach (Toy c in kv.Value)
					{
						int idx = toyIndices.ContainsKey(c) ? toyIndices[c] : -1;
						int ridx = toyIndices.ContainsKey(c) ? sortedReverseIndices[idx] : -1;
						File.AppendAllText(filename, "\"p"+kv.Key+"\" -- \"c"+c+"["+idx+"]"+ridx+"th\"\n");
					}

				}
				File.AppendAllText(filename, "}\n");

			}


			nbToys = nbToysLeft;
            for (int nthIndex = 0; nthIndex < nbToys; ++nthIndex)
            {
                var interestedPeople = perToyInterests[sortedIndices[nthIndex]];
                if (interestedPeople.Count == 0)
                {
                    --nbToysLeft;
                    if (nbPeopleLeft > nbToysLeft) return false;
                }
                else
                {
                    Person luckyPerson = interestedPeople.MinBy( per => needs[per].Count( c => toyIndices.ContainsKey(c)) );
                    foreach (Toy toy in needs[luckyPerson])
                    {
						int unsortedIdx;
						if (toyIndices.TryGetValue(toy, out unsortedIdx))
						{ 
							int sortedIdx = sortedReverseIndices[unsortedIdx];

							if (sortedIdx == nthIndex) {
								toyIndices.Remove(toy);
								if (solution != null) solution.Add(luckyPerson, toy);
							}
							var otherPeopleForOtherToys = perToyInterests[unsortedIdx];
							otherPeopleForOtherToys.Remove(luckyPerson);
							int newCount = otherPeopleForOtherToys.Count;
							// Update ordering
							int newIdx = sortedIdx;
							while (newIdx > nthIndex+1 && newCount < perToyInterests[sortedIndices[newIdx-1]].Count) --newIdx;
							if (newIdx != sortedIdx)
							{
								int tmp = sortedIndices[sortedIdx];
								sortedIndices[sortedIdx] = sortedIndices[newIdx];
								sortedIndices[newIdx] = tmp;

								sortedReverseIndices[sortedIndices[newIdx]] = newIdx;
								sortedReverseIndices[sortedIndices[sortedIdx]] = sortedIdx;
							}
						}
                    }
                    --nbPeopleLeft;
                    --nbToysLeft;
                }
            }

            return true;
        }

        public static Dictionary<int, HashSet<int>> GenerateCase(Random rng, bool isSatisfiable, int nbPeople, out int nbToys)
        {
            nbToys = 0;
            var needs = new Dictionary<int, HashSet<int>>(nbPeople);
            int satBreakIdx = isSatisfiable ? -1 : rng.Next(nbPeople);
            for (int i = 0; i < nbPeople; ++i)
            {
                var intersts = new HashSet<int>();
                if (nbToys > 0)
                {
                    intersts.UnionWith( rng.Repeat(nbToys, rng.Next(nbToys+1)) );
                }
                if (i != satBreakIdx)
                {
                    int nbToAdd = 1;
                    intersts.UnionWith( Enumerable.Range(nbToys, nbToAdd) );
                    nbToys += nbToAdd;
                }
                needs.Add(i,intersts);
            }

            return needs;
        }

        public static bool UnitTest(int minSize, int maxSize)
        {
			Dictionary<int, HashSet<int>> needs;
			needs = new Dictionary<int,HashSet<int>>();
			needs.Add(1, new HashSet<int>(){0,1,2});
			needs.Add(0, new HashSet<int>(){0,1});
			needs.Add(2, new HashSet<int>(){0,2});
            if (!SolveOld(needs, 3))
            {
                throw new Exception();
            }       

            Random rng = new Random(5);
            for (int i=minSize; i<maxSize; ++i) {
                int nbToys;
                needs = GenerateCase(rng, true, i, out nbToys);
                if (!SolveOld(needs, nbToys))
                {
                    throw new Exception();
                }
                needs = GenerateCase(rng, false, i, out nbToys);
                if (SolveOld(needs, nbToys))
                {
                    throw new Exception();
                }
			}
            return true;
        }
    }
}
