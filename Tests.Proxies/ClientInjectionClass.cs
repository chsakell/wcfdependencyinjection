using Data.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Proxies
{
    public class ClientInjectionClass : Disposable
    {
        public Client.Contracts.IBlogService _blogProxy;
        public Client.Contracts.IArticleService _articleProxy;
        public ClientInjectionClass(Client.Contracts.IArticleService articleServiceProxy,
            Client.Contracts.IBlogService blogServiceProxy)
        {
            this._blogProxy = blogServiceProxy;
            this._articleProxy = articleServiceProxy;
        }

        #region IDisposable
        protected override void DisposeCore()
        {
            base.DisposeCore();
            try
            {
                (_blogProxy as Client.Proxies.BlogClient).CleanUp();

                (_articleProxy as Client.Proxies.ArticleClient).CleanUp();
            }
            catch
            {
                _blogProxy = null;
                _articleProxy = null;
            }
        }
        #endregion

        #region Methods

        public Client.Entities.Article[] GetArticles()
        {
            return _articleProxy.GetAll();
        }

        public Client.Entities.Blog GetBlogById(int id)
        {
            return _blogProxy.GetById(id);
        }

        #endregion
    }
}
