using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    /// <summary>
    /// How could we best leverage the power of HK for this puzzle?
    /// HK's power comes from its ability to work without ever backtracking, every step is a good step.
    /// But to manage that, HK arranges displacement though a DFS of linear complexity.
    /// Its not clear that the linear complexity really is the issue here, as long as we can keep track.
    /// What prevents us from stating the puzzle as a bipartite matching?
    /// We can do that for quarters / quaters matching
    /// </summary>
    class GlobalSolver
    {
    }
}
