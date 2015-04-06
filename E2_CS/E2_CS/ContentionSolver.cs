using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace E2_CS
{
    class ContentionSolver
    {
        public static bool Solve<Person,Cookie>(IDictionary<Person, HashSet<Cookie>> needs, int nbCookies, IDictionary<Person,Cookie> solution = null)
        {
			bool save = false;

            var nbPeopleLeft = needs.Count;
            if (nbPeopleLeft > nbCookies) return false;
            var cookieIndices = new Dictionary<Cookie, int>();
            var perCookieInterests = new HashSet<Person>[nbCookies];

            int nbCookiesLeft = 0;
            foreach (KeyValuePair<Person, HashSet<Cookie>> kv in needs)
            {
				if (kv.Value.Count == 1)
				{
					if (solution!=null) solution.Add(kv.Key, kv.Value.First());
					--nbPeopleLeft;
				}
				else
				{
					foreach (Cookie c in kv.Value)
					{
						int index;
						if (cookieIndices.TryGetValue(c, out index) == false) {
							index = nbCookiesLeft++;
							cookieIndices.Add(c, index);
							perCookieInterests[index] = new HashSet<Person>();
						}
						perCookieInterests[index].Add(kv.Key);
					}
				}
            }
            if (nbPeopleLeft > nbCookiesLeft) return false;

            var sortedIndices = Enumerable.Range(0,nbCookiesLeft).ToArray();
            Array.Sort(sortedIndices, Comparer<int>.Create( (a,b) => perCookieInterests[a].Count.CompareTo(perCookieInterests[b].Count) ));
			var sortedReverseIndices = new int[nbCookiesLeft];
			for (int i=0; i<nbCookiesLeft; ++i) sortedReverseIndices[sortedIndices[i]] = i;

			if (save)
			{
				string filename = @"../../../../graph.txt";
				File.WriteAllText(filename, "graph {\n");
				foreach (KeyValuePair<Person, HashSet<Cookie>> kv in needs)
				{
					foreach (Cookie c in kv.Value)
					{
						File.AppendAllText(filename, "\"p"+kv.Key+"\" -- \"c"+c+"["+cookieIndices[c]+"]"+sortedReverseIndices[cookieIndices[c]]+"th\"\n");
					}

				}
				File.AppendAllText(filename, "}\n");

			}


			nbCookies = nbCookiesLeft;
            for (int nthIndex = 0; nthIndex < nbCookies; ++nthIndex)
            {
                var interestedPeople = perCookieInterests[sortedIndices[nthIndex]];
                if (interestedPeople.Count == 0)
                {
                    --nbCookiesLeft;
                    if (nbPeopleLeft > nbCookiesLeft) return false;
                }
                else
                {
                    Person luckyPerson = interestedPeople.MinBy( per => needs[per].Count( c => cookieIndices.ContainsKey(c)) );
                    foreach (Cookie cookie in needs[luckyPerson])
                    {
						int unsortedIdx;
						if (cookieIndices.TryGetValue(cookie, out unsortedIdx))
						{ 
							int sortedIdx = sortedReverseIndices[unsortedIdx];

							if (sortedIdx == nthIndex) {
								cookieIndices.Remove(cookie);
								if (solution != null) solution.Add(luckyPerson, cookie);
							}
							var otherPeopleForOtherCookies = perCookieInterests[unsortedIdx];
							otherPeopleForOtherCookies.Remove(luckyPerson);
							int newCount = otherPeopleForOtherCookies.Count;
							// Update ordering
							int newIdx = sortedIdx;
							while (newIdx > nthIndex+1 && newCount < perCookieInterests[sortedIndices[newIdx-1]].Count) --newIdx;
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
                    --nbCookiesLeft;
                }
            }

            return true;
        }

        public static Dictionary<int, HashSet<int>> GenerateCase(Random rng, bool isSatisfiable, int nbPeople, out int nbCookies)
        {
            nbCookies = 0;
            var needs = new Dictionary<int, HashSet<int>>(nbPeople);
            int satBreakIdx = isSatisfiable ? -1 : rng.Next(nbPeople);
            for (int i = 0; i < nbPeople; ++i)
            {
                var intersts = new HashSet<int>();
                if (nbCookies > 0)
                {
                    intersts.UnionWith( rng.Repeat(nbCookies, rng.Next(nbCookies+1)) );
                }
                if (i != satBreakIdx)
                {
                    int nbToAdd = 1;
                    intersts.UnionWith( Enumerable.Range(nbCookies, nbToAdd) );
                    nbCookies += nbToAdd;
                }
                needs.Add(i,intersts);
            }

            return needs;
        }

        public static bool UnitTest(int minSize, int maxSize)
        {
            Random rng = new Random(5);
            for (int i=minSize; i<maxSize; ++i) {
                int nbCookies;
                Dictionary<int, HashSet<int>> needs;
                needs = GenerateCase(rng, true, i, out nbCookies);
                if (!Solve(needs, nbCookies))
                {
                    throw new Exception();
                }
                needs = GenerateCase(rng, false, i, out nbCookies);
                if (Solve(needs, nbCookies))
                {
                    throw new Exception();
                }
            }
            return true;
        }
    }
}
