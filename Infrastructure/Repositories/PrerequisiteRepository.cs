using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.Models;
using System.Collections.Generic;

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

        public IEnumerable<Prerequisite> GetAll()
        {
            IEnumerable<PrerequisiteModel> prerequisiteModels = _prerequisiteDAL.GetAll();
            return _prerequisiteMapper.MapDataModelsToEntities(prerequisiteModels);
        }
    }
}
