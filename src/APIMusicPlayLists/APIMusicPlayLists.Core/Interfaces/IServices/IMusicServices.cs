using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Interfaces.IServices
{
    public interface IMusicServices
    {
        Task<IEnumerable<Music>> Get();
        Task<Music> GetByIdAsync(int id);
        Task<ResultDTO> PostAsync(MusicDTO entity);
        Task<ResultDTO> PutAsync(MusicDTO entity);
        Task<ResultDTO> PutAsync(Music entity);
        Task<ResultDTO> DeleteAsync(int id);
        Task<ResultDTO> FavoriteSong(int id, int favorite);
    }
}
