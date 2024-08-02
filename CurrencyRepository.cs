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
    [Dependency(typeof(ICurrencyRepository))]
    public class CurrencyRepository : BaseRepository<CurrencyEntity, byte>, ICurrencyRepository
    {
        public async Task<ListQueryResult<CurrencyView>> GetCurrencyAsync(QueryDataSource<CurrencyView> searchQuery, int languageRef)
        {
            var result = new ListQueryResult<CurrencyView>
            {
                Entities = new List<CurrencyView>()
            };

            var queryResult = from c in GenericRepository.Query<CurrencyEntity>()
                              join cl in GenericRepository.Query<CurrencyLanguageEntity>()
                              on c.Key equals cl.CurrencyRef
                              where cl.LanguageRef == languageRef
                              select new CurrencyView
                              {
                                  Key = c.Key,
                                  IsActive = c.IsActive,
                                  Icon = c.Icon,
                                  IsNational = c.IsNational,
                                  CurrencyRef = cl.CurrencyRef,
                                  LanguageRef = cl.LanguageRef,
                                  _Title = cl._Title
                              };

            result = await queryResult.ToListQueryResultAsync(searchQuery);

            return result;
        }
    }
}
