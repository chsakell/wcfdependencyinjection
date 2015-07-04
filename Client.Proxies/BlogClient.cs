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
    public class BlogClient : ClientBase<IBlogService>, IBlogService
    {
        #region IBlogService implementation
        public void Add(Blog blog)
        {
            Channel.Add(blog);
        }

        public void Update(Blog blog)
        {
            Channel.Update(blog);
        }

        public void Delete(Blog blog)
        {
            Channel.Delete(blog);
        }

        public Blog GetById(int id)
        {
            return Channel.GetById(id);
        }

        public Blog[] GetAll()
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
