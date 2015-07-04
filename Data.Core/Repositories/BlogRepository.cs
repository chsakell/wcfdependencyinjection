using Business.Entities;
using Data.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Core.Repositories
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Blog GetBlogByName(string blogName)
        {
            var _blog = this.DbContext.BlogSet.Where(b => b.Name == blogName).FirstOrDefault();

            return _blog;
        }
    }

    public interface IBlogRepository : IRepository<Blog>
    {
        Blog GetBlogByName(string blogName);
    }
}
