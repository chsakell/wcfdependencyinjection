using Autofac;
using Autofac.Integration.Wcf;
using Business.Services;
using Data.Core.Infrastructure;
using Data.Core.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Moq.Language;
using System;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Client.Proxies;

namespace Tests.Proxies
{
    public static class Bootstrapper
    {/// <summary>
        /// Configures and builds Autofac IOC container.
        /// </summary>
        /// <returns></returns>
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            // register services
            builder.RegisterType<BlogService>().As<Business.Services.Contracts.IBlogService>();
            builder.RegisterType<ArticleService>().As<Business.Services.Contracts.IArticleService>();

            // register proxies
            builder.Register(c => new ChannelFactory<Client.Contracts.IArticleService>("BasicHttpBinding_IArticleService"))
                .InstancePerLifetimeScope();
            builder.Register(c => new ChannelFactory<Client.Contracts.IBlogService>("BasicHttpBinding_IBlogService"))
                .InstancePerLifetimeScope();

            builder.RegisterType<ArticleClient>().As<Client.Contracts.IArticleService>().UseWcfSafeRelease();
            builder.RegisterType<BlogClient>().As<Client.Contracts.IBlogService>().UseWcfSafeRelease();

            // Unit of Work
            var _unitOfWork = new Mock<IUnitOfWork>();
            builder.RegisterInstance(_unitOfWork.Object).As<IUnitOfWork>();
            // DbFactory
            var _dbFactory = new Mock<IDbFactory>();
            builder.RegisterInstance(_dbFactory.Object).As<IDbFactory>();

            //Repositories
            var _articlesRepository = new Mock<IArticleRepository>();
            _articlesRepository.Setup(x => x.GetAll()).Returns(new List<Business.Entities.Article>()
                {
                    new Business.Entities.Article() { 
                        ID = 1, 
                        Author = "Chris Sakellarios", 
                        BlogID = 1, 
                        Contents = "Dependency injection is a software design pattern that implements..", 
                        Title = "WCF Dependency Injection", 
                        URL =" http://chsakell.com/2015/07/03/dependency-injection-in-wcf/"}
                });
            builder.RegisterInstance(_articlesRepository.Object).As<IArticleRepository>();

            var _blogsRepository = new Mock<IBlogRepository>();
            _blogsRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Business.Entities.Blog>(
                    id => new Business.Entities.Blog() { 
                        ID = id,
                        Name = "chsakell's blog",
                        Owner = "Chris Sakellarios",
                        URL = "http://chsakell.com/"
                    }));
            builder.RegisterInstance(_blogsRepository.Object).As<IBlogRepository>();

            builder.RegisterType<ClientInjectionClass>();

            // build container
            return builder.Build();
        }
    }
}