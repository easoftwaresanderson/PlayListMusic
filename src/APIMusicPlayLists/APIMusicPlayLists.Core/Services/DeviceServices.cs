using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Infra.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Core.Services
{
    public class DeviceServices : IDeviceServices
    {
        private readonly IRepositoryBase<Device> _repository;

        public DeviceServices(IRepositoryBase<Device> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Device>> Get()
        {

            var data = await _repository.List();
            return data;

        }

        public async Task<Device> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Device> GetByUniqueDeviceID(string id)
        {
            var data = _repository.Query().AsQueryable().Where(d => d.UniqueID.Equals(id)).FirstOrDefault();
            return data;
        }

        public async Task<ResultDTO> PostAsync(DeviceDTO entity)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Post Device";

                Device reg = new Device
                {
                    DeviceType = entity.DeviceType,
                    Idiom = entity.Idiom,
                    Manufacturer = entity.Manufacturer,
                    Model = entity.Model,
                    Name = entity.Name,
                    Platform = entity.Platform,
                    UniqueID = entity.UniqueID,
                    VersionString = entity.VersionString
                };


                var data = await _repository.AddAsync(reg);
                res.ReturnID = reg.Id.ToString();

                return res;
            }
            catch (Exception ex)
            {
                res.Errors.Add(ex.Message);
                return res;
            }
        }

        public async Task<ResultDTO> PutAsync(DeviceDTO entity)
        {
            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Put Device";

                Device reg = new Device
                {
                    DeviceType = entity.DeviceType,
                    Id = entity.Id,
                    Idiom = entity.Idiom,
                    Manufacturer = entity.Manufacturer,
                    Model = entity.Model,
                    Name = entity.Name,
                    Platform = entity.Platform,
                    UniqueID = entity.UniqueID,
                    VersionString = entity.VersionString
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
        public async Task<ResultDTO> DeleteAsync(int id)
        {

            ResultDTO res = new ResultDTO();

            try
            {
                res.Action = "Delete Device";

                var album = await _repository.GetByIdAsync(id);

                if (album == null)
                {
                    res.Errors.Add("Device não encontrado para deletar");
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
