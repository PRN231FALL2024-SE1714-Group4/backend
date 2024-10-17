using BOs.DTOS;
using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IHistoryService
    {
        Task<List<History>> GetAllHistoriesAsync();
        Task<History> GetHistoryByIdAsync(Guid historyId);
        Task<History> CreateHistoryAsync(HistoryCreateRequest request);
        Task<History> UpdateHistoryAsync(Guid historyId, HistoryUpdateRequest request);
        Task<bool> DeleteHistoryAsync(Guid historyId);
    }
}
