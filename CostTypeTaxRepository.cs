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
    [Dependency(typeof(ICostTypeTaxRepository))]
    public class CostTypeTaxRepository : BaseRepository<CostTypeTaxEntity, int>, ICostTypeTaxRepository
    {
        public async Task<ListQueryResult<CostTypeTaxView>> GetCostTypeTaxAsync(QueryDataSource<CostTypeTaxView> searchQuery, int languageRef, short costTypeRef)
        {
            var result = new ListQueryResult<CostTypeTaxView>
            {
                Entities = new List<CostTypeTaxView>()
            };

            var queryResult = from t in GenericRepository.Query<TaxEntity>()

                              join tl in GenericRepository.Query<TaxLanguageEntity>().Where(x => x.LanguageRef == languageRef) on t.Key equals tl.TaxRef

                              join ctt in GenericRepository.Query<CostTypeTaxEntity>().Where(x => x.CostTypeRef == costTypeRef) on t.Key equals ctt.TaxRef
                              into leftCtt
                              from lctt in leftCtt.DefaultIfEmpty()

                              select new CostTypeTaxView
                              {
                                  Key = t.Key,
                                  TaxPercent = t.TaxPercent,
                                  Alias = t.Alias,
                                  IsActive = t.IsActive,
                                  LanguageRef = tl.LanguageRef,
                                  _Title = tl._Title,
                                  _Description = tl._Description,
                                  CostTypeRef = lctt.CostTypeRef,
                                  TaxRef = lctt.TaxRef
                              };

            result = await queryResult.ToListQueryResultAsync(searchQuery);

            return result;
        }
    }
}
