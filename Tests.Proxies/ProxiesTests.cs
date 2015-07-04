using Autofac;
using Autofac.Integration.Wcf;
using Business.Services;
using Client.Proxies;
using Core.Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Proxies
{
    [TestFixture]
    public class ProxiesTests
    {
        IContainer container = null;
        ServiceHost svcArticleHost = null;
        ServiceHost svcBlogHost = null;
        Uri svcArticleServiceURI = new Uri("http://localhost:18850/ArticleService.svc");
        Uri svcBlogServiceURI = new Uri("http://localhost:18850/BlogService.svc");
        

        [SetUp]
        public void Setup()
        {
            try
            {
                container = Bootstrapper.BuildContainer();

                svcArticleHost = new ServiceHost(typeof(ArticleService), svcArticleServiceURI);
                svcBlogHost = new ServiceHost(typeof(BlogService), svcBlogServiceURI);

                svcArticleHost.AddDependencyInjectionBehavior<Business.Services.Contracts.IArticleService>(container);
                svcBlogHost.AddDependencyInjectionBehavior<Business.Services.Contracts.IBlogService>(container);

                svcArticleHost.Open();
                svcBlogHost.Open();
            }
            catch (Exception ex)
            {
                svcArticleHost = null;
                svcBlogHost = null;
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (svcArticleHost != null && svcArticleHost.State != CommunicationState.Closed)
                    svcArticleHost.Close();

                if (svcBlogHost != null && svcBlogHost.State != CommunicationState.Closed)
                    svcBlogHost.Close();
            }
            catch (Exception ex)
            {
                svcArticleHost = null;
                svcBlogHost = null;
            }
            finally
            {
                svcArticleHost = null;
                svcBlogHost = null;
            }
        }

        [Test]
        public void test_self_host_connection()
        {
            Assert.That(svcArticleHost.State, Is.EqualTo(CommunicationState.Opened));
            Assert.That(svcBlogHost.State, Is.EqualTo(CommunicationState.Opened));
        }

        [Test]
        public void test_article_proxy_is_injected()
        {
            using (var lifetime = container.BeginLifetimeScope())
            {
                Client.Contracts.IArticleService proxy
                = container.Resolve<Client.Contracts.IArticleService>();

                Assert.IsTrue(proxy is ArticleClient);
            }
        }

        [Test]
        public void test_blog_proxy_is_injected()
        {
            using (var lifetime = container.BeginLifetimeScope())
            {
                Client.Contracts.IBlogService proxy
                = container.Resolve<Client.Contracts.IBlogService>();

                Assert.IsTrue(proxy is BlogClient);
            }
        }

        [Test]
        public void test_article_proxy_state()
        {
            Client.Contracts.IArticleService proxy;

            using (var lifetime = container.BeginLifetimeScope())
            {
                proxy = container.Resolve<Client.Contracts.IArticleService>();

                CommunicationState state = (proxy as ArticleClient).State;
                Assert.That(state, Is.EqualTo(CommunicationState.Created));

                // Open connection
                (proxy as ArticleClient).Open();
                Assert.That((proxy as ArticleClient).State, Is.EqualTo(CommunicationState.Opened));
                // Close connection
                (proxy as ArticleClient).Close();
                Assert.That((proxy as ArticleClient).State, Is.EqualTo(CommunicationState.Closed));
            }
        }

        [Test]
        public void test_article_proxy_getall()
        {
            Client.Contracts.IArticleService proxy;
            Client.Entities.Article[] articles = null;

            using (var lifetime = container.BeginLifetimeScope())
            {
                proxy = container.Resolve<Client.Contracts.IArticleService>();

                articles = proxy.GetAll();
            }

            Assert.That(articles.Count(), Is.EqualTo(1));

            // Close connection
            if ((proxy as ArticleClient).State == CommunicationState.Opened)
                (proxy as ClientBase<Client.Contracts.IArticleService>).Close();
        }

        [Test]
        public void test_constructor_injected_proxy()
        {
            ClientInjectionClass _testClass = null;

            using (var lifetime = container.BeginLifetimeScope())
            {
                using (_testClass = new ClientInjectionClass(container.Resolve<Client.Contracts.IArticleService>(),
                   container.Resolve<Client.Contracts.IBlogService>()))
                {
                    {
                        Client.Entities.Article[] _articles = _testClass.GetArticles();
                        Client.Entities.Blog _blog = _testClass.GetBlogById(1);

                        Assert.That(_articles.Count(), Is.EqualTo(1));
                        Assert.That(_blog, Is.Not.Null);
                        Assert.That(_blog.IsValid, Is.EqualTo(true));
                    }
                }
            }

            Assert.That((_testClass._articleProxy as ArticleClient).State, Is.EqualTo(CommunicationState.Closed));
            Assert.That((_testClass._blogProxy as BlogClient).State, Is.EqualTo(CommunicationState.Closed));
        }

        [Test]
        public void test_article_extension_data_not_empty()
        {
            Client.Contracts.IArticleService proxy;
            Client.Entities.Article[] articles = null;

            using (var lifetime = container.BeginLifetimeScope())
            {
                proxy = container.Resolve<Client.Contracts.IArticleService>();

                articles = proxy.GetAll();
            }

            Assert.That(articles.Count(), Is.EqualTo(1));

            var contentLength = Extensions.GetExtensionDataMemberValue(articles.First(), "ContentLength");

            Assert.That(articles.First().Contents.Length, Is.EqualTo(Int32.Parse(contentLength.ToString())));
        }
    }
}
