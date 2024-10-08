using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Response
{
    public class WorkResponse : Work
    {
        public int compltedTask { get; set; }
        public int totalTask { get; set; }
        public Area Area { get; set; }
    }
}
