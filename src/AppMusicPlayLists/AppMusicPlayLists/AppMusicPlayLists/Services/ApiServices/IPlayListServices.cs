using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMusicPlayLists.Services.ApiServices
{
    public interface IPlayListServices
    {
        Task<ResultDTO> AddItemAsync(PlayListDTO item);
        Task<ResultDTO> UpdateItemAsync(PlayListDTO item);
        Task<ResultDTO> DeleteItemAsync(int id);
        Task<PlayListDTO> GetItemAsync(int id);
        Task<PlayListDTO> GetPlayListByDeviceIDAsync(int id);
        Task<IEnumerable<PlayListDTO>> GetItemsAsync(bool forceRefresh = false);

        Task<ResultDTO> FavoriteSong(PlayListFavoriteCommand command);

    }
}
