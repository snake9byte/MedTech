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
using ICD.Base.Domain.External_Entity.HRM;

namespace ICD.Base.Repository
{
    [Dependency(typeof(IGLNRepository))]
    public class GLNRepository : BaseRepository<GLNEntity, long>, IGLNRepository
    {
        public async Task<ListQueryResult<GLNView>> GetGLNAsync(QueryDataSource<GLNView> searchQuery, int languageRef)
        {
            var result = new ListQueryResult<GLNView>
            {
                Entities = new List<GLNView>()
            };

            var queryResult = from g in GenericRepository.Query<GLNEntity>()

                              join gl in GenericRepository.Query<GLNLanguageEntity>() on g.Key equals gl.GLNRef

                              join pl in GenericRepository.Query<PersonLanguageEntity>() on g.PersonRef equals pl.PersonRef

                              where gl.LanguageRef == languageRef &&
                              pl.LanguageRef == languageRef

                              select new GLNView
                              {
                                  Key = g.Key,
                                  PersonRef = g.PersonRef,
                                  GLN = g.GLN,
                                  GLNLanguageRef = gl.Key,
                                  _Title = gl._Title,
                                  _Address = gl._Address,
                                  FullName = pl.FullName
                              };

            result = await queryResult.ToListQueryResultAsync(searchQuery);

            return result;
        }

        public async Task<ListQueryResult<GLNCoView>> GetGLNsOfCompanyAsync(QueryDataSource<GLNCoView> searchQuery, int languageRef)
        {
            var result = new ListQueryResult<GLNCoView>
            {
                Entities = new List<GLNCoView>()
            };

            var queryResult = from g in GenericRepository.Query<GLNEntity>()

                              join gl in GenericRepository.Query<GLNLanguageEntity>() on g.Key equals gl.GLNRef

                              join c in GenericRepository.Query<CompanyEntity>() on g.PersonRef equals c.PersonRef

                              join cl in GenericRepository.Query<CompanyLanguageEntity>() on c.Key equals cl.CompanyRef

                              where gl.LanguageRef == languageRef &&
                              cl.LanguageRef == languageRef

                              select new GLNCoView
                              {
                                  Key = g.Key,
                                  PersonRef = g.PersonRef,
                                  GLN = g.GLN,
                                  LanguageRef = gl.LanguageRef,
                                  _Title = gl._Title,
                                  _Address = gl._Address,
                                  CompanyKey = c.Key,
                                  Company_Title = cl._Title,
                                  GLN_TitleGLN = gl._Title + " - " + g.GLN
                              };

            result = await queryResult.ToListQueryResultAsync(searchQuery);

            return result;
        }
    }
}
