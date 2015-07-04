using Business.Entities;
using Business.Services.Contracts;
using Data.Core.Infrastructure;
using Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogRepository _blogRepository;

        public BlogService(IUnitOfWork unitOfWork, IBlogRepository blogRepository)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
        }
        public void Add(Blog blog)
        {
            _blogRepository.Add(blog);
            _unitOfWork.Commit();
        }

        public void Update(Blog blog)
        {
            _blogRepository.Update(blog);
            _unitOfWork.Commit();
        }

        public void Delete(Blog blog)
        {
            _blogRepository.Delete(blog);
            _unitOfWork.Commit();
        }

        public Blog GetById(int id)
        {
            return _blogRepository.GetById(id);
        }

        public Blog[] GetAll()
        {
            return _blogRepository.GetAll().ToArray();
        }
    }
}
