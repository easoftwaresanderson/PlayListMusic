using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMusicPlayLists.Services.ApiServices
{
    public interface IMusicServices
    {
        Task<MusicDTO> GetItemAsync(int id);
        Task<IEnumerable<MusicDTO>> GetItemsAsync(bool forceRefresh = false);
    }
}
