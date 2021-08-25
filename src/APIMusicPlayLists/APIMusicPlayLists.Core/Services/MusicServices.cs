using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Services
{
    public class MusicServices : IMusicServices
    {
        private readonly IRepositoryBase<Music> _repository;
  

        public MusicServices(IRepositoryBase<Music> repository)
        {
            _repository = repository;
      
        }


        public async Task<IEnumerable<Music>> Get()
        {
            var data = await _repository.List();
            return data;
        }

        public async Task<Music> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

      

        public async Task<ResultDTO> PostAsync(MusicDTO data)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Post Music";

                Music reg = new Music
                {
                    AlbumImage = data.AlbumImage,
                    AlbumName = data.AlbumName,
                    AlbumYear = data.AlbumYear,
                    AlbumNotes = data.AlbumNotes,
                    ArtistName = data.ArtistName,
                    MusicName = data.MusicName
                };

                var ret = await _repository.AddAsync(reg);

                res.ReturnID = reg.Id.ToString();

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }


        }

        public async Task<ResultDTO> PutAsync(MusicDTO data)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Put Music";

                Music reg = new Music
                {
                    Id = data.Id,
                    AlbumImage = data.AlbumImage,
                    AlbumName = data.AlbumName,
                    AlbumYear = data.AlbumYear,
                    AlbumNotes = data.AlbumNotes,
                    ArtistName = data.ArtistName,
                    MusicName = data.MusicName
                };

                await _repository.UpdateAsync(reg);

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }

        }

        public async Task<ResultDTO> PutAsync(Music data)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Put Music";
                await _repository.UpdateAsync(data);

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }

        }

        public async Task<ResultDTO> DeleteAsync(int id)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Delete Music";

                var music = await _repository.GetByIdAsync(id);

                if (music == null)
                {
                    res.Errors.Add("Music não encontrado para deletar");
                    return res;
                }

                await _repository.DeleteAsync(music);

                return res;

            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }
        }

        public async Task<ResultDTO> FavoriteSong(int id,int favorite)
        {

            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Favorite / UnFavorite Music";

                var music = await GetByIdAsync(id);

                if (music == null)
                {
                    res.Errors.Add("Music not found to favorite.");
                    return res;
                }

                music.Favorite = favorite;

                await _repository.UpdateAsync(music);

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }

        }


    }
}
