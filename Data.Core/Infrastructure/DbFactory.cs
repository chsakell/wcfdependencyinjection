using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Core.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        DIContext dbContext;

        public DIContext Init()
        {
            return dbContext ?? (dbContext = new DIContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
