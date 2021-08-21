using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMusicPlayLists.Services.ApiServices
{
    public interface IDeviceServices
    {
        Task<ResultDTO> AddItemAsync(DeviceDTO item);
        Task<ResultDTO> UpdateItemAsync(DeviceDTO item);
        Task<ResultDTO> DeleteItemAsync(int id);
        Task<DeviceDTO> GetItemAsync(int id);
        Task<DeviceDTO> GetDeviceByUniqueID(string id);
        Task<IEnumerable<DeviceDTO>> GetItemsAsync(bool forceRefresh = false);
    }
}
