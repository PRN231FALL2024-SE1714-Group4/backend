using BOs;
using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Response
{
    public class UserShiftTimeResponse
    {
        public User user {  get; set; }
        public WorkShiftEnum workShift { get; set; }
        public DateOnly date { get; set; }
        public float EstimateTime { get; set; }
        public List<WorkMission> WorkMissions { get; set; }
    }
}
