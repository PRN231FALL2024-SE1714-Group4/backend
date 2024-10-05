using BOs;
using BOs.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IAreaService
    {
        List<Area> GetAreas();
        Area GetArea(Guid id);
        Area CreateArea(AreaCreateRequest request);
        Area UpdateArea(Guid id, AreaUpdateRequest request);
        bool DeleteArea(Guid id);
    }
}
