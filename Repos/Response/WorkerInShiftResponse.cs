using BOs;
using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Response
{
    public class WorkerInShiftResponse
    {
        public WorkShiftEnum workShift { get; set; }
        public int countOfWorker { get; set; }
        public List<User> users { get; set; }
        public DateOnly date { get; set; }
    }
}
