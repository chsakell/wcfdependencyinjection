using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Core
{
    public class DbInitializer : CreateDatabaseIfNotExists<DIContext>
    {
        protected override void Seed(DIContext context)
        {
            base.Seed(context);

            context.BlogSet.Add(new Business.Entities.Blog
            {
                Name = "chsakell's blog",
                URL = "http://chsakell.com",
                Owner = "Christos Sakellarios"
            });

            context.ArticleSet.Add(new Business.Entities.Article
            {
                Title = "WCF Dependency Injection",
                Contents = "Dependency injection is a software design pattern that implements..",
                Author = "Christos Sakellarios",
                URL = "https://chsakell.com/2015/07/03/dependency-injection-in-wcf/",
                BlogID = 1
            });

            context.SaveChanges();

        }
    }
}
