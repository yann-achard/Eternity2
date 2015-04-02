using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    class ContentionSolver
    {
        public static bool IsSatisfiable<Person,Cookie>(IDictionary<Person, HashSet<Cookie>> needs, int nbCookies)
        {
            var nbPeopleLeft = needs.Count;
            if (nbPeopleLeft > nbCookies) return false;
            var cookieIndices = new Dictionary<Cookie, int>();
            var perCookieInterests = new HashSet<Person>[nbCookies];
            perCookieInterests.FillWith( () => new HashSet<Person>() );

            nbCookies = 0;
            foreach (KeyValuePair<Person, HashSet<Cookie>> kv in needs)
            {
                foreach (Cookie c in kv.Value)
                {
                    int index;
                    if (cookieIndices.TryGetValue(c, out index) == false) {
                        index = nbCookies++;
                        cookieIndices.Add(c, index);
                    }
                    perCookieInterests[index].Add(kv.Key);
                }
            }
            if (nbPeopleLeft > nbCookies) return false;

            var sortedIndices = Enumerable.Range(0,nbCookies).ToArray();
            Array.Sort(sortedIndices, Comparer<int>.Create( (a,b) => perCookieInterests[a].Count.CompareTo(perCookieInterests[b].Count) ));

            int nbCookiesLeft = nbCookies;
            for (int cookieIndex = 0; cookieIndex < nbCookies; ++cookieIndex)
            {
                var interestedPeople = perCookieInterests[cookieIndex];
                if (interestedPeople.Count == 0)
                {
                    --nbCookiesLeft;
                    if (nbPeopleLeft > nbCookies) return false;
                }
                else
                {
                    var luckyPerson = interestedPeople.First();
                    --nbPeopleLeft;
                    foreach (Cookie cookie in needs[luckyPerson])
                    {
                        int currIdx = cookieIndices[cookie];
                        var otherPeopleForOtherCookies = perCookieInterests[currIdx];
                        otherPeopleForOtherCookies.Remove(luckyPerson);
                        int newCount = otherPeopleForOtherCookies.Count;
                        // Update ordering
                        int nextIdx = currIdx;
                        while (nextIdx > cookieIndex && perCookieInterests[nextIdx-1].Count > newCount) --nextIdx;
                        if (nextIdx != currIdx)
                        {
                            perCookieInterests[currIdx] = perCookieInterests[nextIdx];
                            perCookieInterests[nextIdx] = otherPeopleForOtherCookies;
                        }
                    }
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
                if (!IsSatisfiable(needs, nbCookies))
                {
                    throw new Exception();
                }
                needs = GenerateCase(rng, false, i, out nbCookies);
                if (IsSatisfiable(needs, nbCookies))
                {
                    throw new Exception();
                }
            }
            return true;
        }
    }
}
