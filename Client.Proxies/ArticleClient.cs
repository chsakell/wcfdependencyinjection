using Client.Contracts;
using Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Proxies
{
    public class ArticleClient : ClientBase<IArticleService>, IArticleService
    {
        #region IArticleService Members
        public void Add(Article article)
        {
            Channel.Add(article);
        }

        public void Update(Article article)
        {
            Channel.Update(article);
        }

        public void Delete(Article article)
        {
            Channel.Delete(article);
        }

        public Article GetById(int id)
        {
            return Channel.GetById(id);
        }

        public Article[] GetAll()
        {
            return Channel.GetAll();
        }
        #endregion 

        public void CleanUp()
        {
            try
            {
                if (base.State != CommunicationState.Faulted)
                    base.Close();
                else
                    base.Abort();
            }
            catch (Exception ex)
            {
                base.Abort();
            }
        }
    }
}
