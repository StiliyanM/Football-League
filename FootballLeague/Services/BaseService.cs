namespace FootballLeague.Services
{
    using AutoMapper;
    using FootballLeague.Common;
    using System;

    public class BaseService
    {
        protected readonly IMapper mapper;

        public BaseService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected void EnsureExists(object entity, string entityName)
        {
            if (entity == null)
            {
                throw new ArgumentException(string.Format(Constants.ENTITY_DOES_NOT_EXIST, entityName));
            }
        }
    }
}
