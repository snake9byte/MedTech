using ICD.Base.Domain.Entity;
using ICD.Base.Domain.View;
using ICD.Base.RepositoryContract;
using ICD.Framework.Data.Repository;
using ICD.Framework.DataAnnotation;
using ICD.Framework.Model;
using ICD.Framework.QueryDataSource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ICD.Framework.Extensions;

namespace ICD.Base.Repository
{
    [Dependency(typeof(ICountryRepository))]
    public class CountryRepository : BaseRepository<CountryEntity, int>, ICountryRepository
    {
        public async Task<ListQueryResult<CountryView>> GetCountryAsync(QueryDataSource<CountryView> searchQuery, int languageRef)
        {
            var finalResult = new ListQueryResult<CountryView>
            {
                Entities = new List<CountryView>()
            };

            var queryResult = from c in GenericRepository.Query<CountryEntity>()
                              join cl in GenericRepository.Query<CountryLanguageEntity>()
                              on c.Key equals cl.CountryRef
                              where cl.LanguageRef == languageRef
                              select new CountryView
                              {
                                  Key = c.Key,
                                  IsActive = c.IsActive,
                                  CountryRef = cl.CountryRef,
                                  LanguageRef = cl.LanguageRef,
                                  _Title = cl._Title
                              };

            var result = await queryResult.ToListQueryResultAsync(searchQuery);

            finalResult = result;

            return finalResult;
        }
    }
}
