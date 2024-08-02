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
    [Dependency(typeof(IExpenseCenterLanguageRepository))]
    public class ExpenseCenterLanguageRepository : BaseRepository<ExpenseCenterLanguageEntity, long>, IExpenseCenterLanguageRepository
    {
        public async Task<ListQueryResult<ExpenseCenterLanguageEntity>> GetExpenseCenterLanguagesAsync(QueryDataSource<ExpenseCenterLanguageEntity> queryDataSource, int languageRef)
        {
            var result = new ListQueryResult<ExpenseCenterLanguageEntity>
            {
                Entities = new List<ExpenseCenterLanguageEntity>()
            };

            var queryResult = from ecl in GenericRepository.Query<ExpenseCenterLanguageEntity>()
                              where ecl.LanguageRef == languageRef
                              select ecl;

            result = await queryResult.ToListQueryResultAsync(queryDataSource);

            return result;
        }
    }
}
