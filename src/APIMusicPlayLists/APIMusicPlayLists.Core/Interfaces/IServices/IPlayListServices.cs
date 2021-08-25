using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Interfaces.IServices
{
    public interface IPlayListServices
    {
        Task<IEnumerable<PlayList>> Get();
        Task<PlayList> GetByIdAsync(int id);
        Task<PlayList> GetByDeviceIdAsync(int DeviceID);
        Task<ResultDTO> PostAsync(PlayListDTO entity);
        Task<ResultDTO> PutAsync(PlayListDTO entity);
        Task<ResultDTO> DeleteAsync(int id);
        Task<ResultDTO> FavoriteSong(PlayListFavoriteCommand command);
    }

}
