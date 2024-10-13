using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class UserShiftRequest
    {
        public Guid? UserId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkShiftEnum WorkShift { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
