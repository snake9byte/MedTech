using System.Collections.Generic;
using System.Threading.Tasks;
using ICD.Base.Domain.Entity;
using ICD.Base.Domain.View;
using ICD.Base.RepositoryContract;
using ICD.Framework.Data.Repository;
using ICD.Framework.DataAnnotation;
using ICD.Framework.Model;
using ICD.Framework.QueryDataSource;
using System.Linq;
using ICD.Framework.Extensions;

namespace ICD.Base.Repository
{
    [Dependency(typeof(IExpenseCenterRepository))]
    public class ExpenseCenterRepository : BaseRepository<ExpenseCenterEntity, long>, IExpenseCenterRepository
    {
        public async Task<ListQueryResult<ExpenseCenterView>> GetExpenseCentersAsync(QueryDataSource<ExpenseCenterView> queryDataSource , int languageRef)
        {
            var result = new ListQueryResult<ExpenseCenterView>
            {
                Entities = new List<ExpenseCenterView>()
            };

            var queryResult = from ec in GenericRepository.Query<ExpenseCenterEntity>()
                              join ecl in GenericRepository.Query<ExpenseCenterLanguageEntity>()
                              on ec.Key equals ecl.ExpenseCenterRef
                              where ecl.LanguageRef == languageRef
                              select new ExpenseCenterView
                              {
                                  Key = ec.Key,
                                  CompanyRef = ec.CompanyRef,
                                  IsActive = ec.IsActive,
                                  _Title = ecl._Title,
                                  LanguageRef = ecl.LanguageRef
                              };

            result = await queryResult.ToListQueryResultAsync(queryDataSource);

            return result;
        }
    }
}
