using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly PrerequisiteDAL _prerequisiteDAL;
        private readonly MapperBase<Prerequisite, PrerequisiteModel> _prerequisiteMapper;

        public PrerequisiteRepository(PrerequisiteMapper prerequisiteMapper, PrerequisiteDAL prerequisiteDAL)
        {
            _prerequisiteMapper = prerequisiteMapper;
            _prerequisiteDAL = prerequisiteDAL;
        }

        public async Task<IEnumerable<Prerequisite>> GetAllAsync()
        {
            IEnumerable<PrerequisiteModel> prerequisiteModels = await _prerequisiteDAL.GetAllAsync();
            return _prerequisiteMapper.MapDataModelsToEntities(prerequisiteModels);
        }
    }
}
