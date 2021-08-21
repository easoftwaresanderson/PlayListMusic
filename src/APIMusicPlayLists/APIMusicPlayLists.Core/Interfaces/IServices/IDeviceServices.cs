using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Interfaces.IServices
{
    public interface IDeviceServices
    {
        Task<IEnumerable<Device>> Get();
        Task<Device> GetByIdAsync(int id);

        Task<Device> GetByDeviceIdAsync(string id);
        Task<ResultDTO> PostAsync(DeviceDTO entity);
        Task<ResultDTO> PutAsync(DeviceDTO entity);
        Task<ResultDTO> DeleteAsync(int id);
    }
}
