using ElMaitre.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }


        public static Blog ToBlog(BlogDTO blog)
        {

            return new Blog
            {
                Id = blog.Id,
                Name = blog.Name,
                NameEn = blog.NameEn,
                ImagePath = blog.ImagePath,
                Description = blog.Description,
            };
        }

        public static BlogDTO ToBlogDTO(Blog blog)
        {

            return new BlogDTO
            {
                Id = blog.Id,
                Name = blog.Name,
                NameEn = blog.NameEn,
                ImagePath = blog.ImagePath,
                Description = blog.Description,
                DescriptionEn = blog.DescriptionEn,

            };
        }
    }
}
