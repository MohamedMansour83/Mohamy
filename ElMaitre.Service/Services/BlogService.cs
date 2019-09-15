using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using ElMaitre.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElMaitre.Services
{
    public class BlogService : IBlogService
    {
        private IRepository<Blog> BlogRepository;

        public BlogService(IRepository<Blog> BlogRepository)
        {
            this.BlogRepository = BlogRepository;
        }

        public IEnumerable<BlogDTO> Get()
        {
            
            return BlogRepository.Get().Select(s=> BlogDTO.ToBlogDTO(s));
        }

        public BlogDTO Get(int Id)
        {
            return BlogDTO.ToBlogDTO(BlogRepository.Get(Id));
        }

    }
}
