using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Services
{
    public class PlayListServices : IPlayListServices
    {
        private readonly IRepositoryBase<PlayList> _repository;
        private readonly IRepositoryBase<Music> _musicrepository;

        private MusicServices _musicServices;
        public PlayListServices(IRepositoryBase<PlayList> repository, IRepositoryBase<Music> musicrepository)
        {
            _repository = repository;
            _musicrepository = musicrepository;
            _musicServices = new MusicServices(musicrepository);
        }

        public async Task<IEnumerable<PlayList>> Get()
        {
            var data = await _repository.List();

            return data;
        }

        public async Task<PlayList> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<PlayList> GetByDeviceIdAsync(int id)
        {
            var data = _repository.Query().AsQueryable().Where(d => d.DeviceId == id).FirstOrDefault();
            return data;
        }

        public async Task<ResultDTO> FavoriteSong(PlayListFavoriteCommand command)
        {
            ResultDTO res = new ResultDTO();
            res.Action = "Favorite/UnFavorite Music";

            try
            {
                var playList = await GetByIdAsync(command.PlayListId);

                if (playList == null)
                {
                    res.Errors.Add("Play list not found to favorite music.");
                    return res;
                }


                var music = await _musicServices.GetByIdAsync(command.MusicId);
                if (music == null)
                {
                    res.Errors.Add("Music not found to favorite.");
                    return res;
                }

                if (command.Favorite == 1)
                {
                    if (playList.PlayListMusics == null)
                    {
                        playList.PlayListMusics = new List<Music>();
                        playList.PlayListMusics.Add(music);
                    }
                }
                else
                {
                    if (playList.PlayListMusics != null)
                    {
                        playList.PlayListMusics.Remove(music);
                    }
                }

                await _repository.UpdateAsync(playList);
                await _musicServices.FavoriteSong(command.MusicId,command.Favorite);


                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }

        }

        public async Task<ResultDTO> PostAsync(PlayListDTO entity)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Post PlayList";

                PlayList reg = new PlayList
                {
                    PlayListName = entity.PlayListName,
                    DeviceId = entity.Device.Id

                };


                foreach (MusicDTO item in entity.PlayListMusics)
                {
                    reg.PlayListMusics.Add(new Music
                    {
                        Id = item.Id,
                        MusicName = item.MusicName,
                        AlbumImage = item.AlbumImage,
                        AlbumName = item.AlbumName,
                        AlbumNotes = item.AlbumNotes,
                        AlbumYear = item.AlbumYear,
                        ArtistName = item.ArtistName,
                    });
                }

                var data = await _repository.AddAsync(reg);
                res.ReturnID = entity.Id.ToString();

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }
        }

        public async Task<ResultDTO> PutAsync(PlayListDTO entity)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Put PlayList";

                var reg = await GetByIdAsync(entity.Id);
                if (reg == null)
                {
                    res.Errors.Add("PlayList not found to update");
                    return res;
                }

                reg.PlayListName = entity.PlayListName;
                reg.DeviceId = entity.Device.Id;


                reg.PlayListMusics.Clear();

                foreach (MusicDTO item in entity.PlayListMusics)
                {
                    reg.PlayListMusics.Add(new Music
                    {
                        Id = item.Id,
                        MusicName = item.MusicName,
                        AlbumImage = item.AlbumImage,
                        AlbumName = item.AlbumName,
                        AlbumNotes = item.AlbumNotes,
                        AlbumYear = item.AlbumYear,
                        ArtistName = item.ArtistName,
                    });
                }

                await _repository.UpdateAsync(reg);

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
                res.Action = "Delete PlayList";

                var album = await _repository.GetByIdAsync(id);

                if (album == null)
                {
                    res.Errors.Add("PlayList não encontrado para deletar");
                    return res;
                }

                await _repository.DeleteAsync(album);

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
