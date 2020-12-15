using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        private BlogContext context;

        public EfBlogRepository(BlogContext _context)
        {
            context = _context;
        }

        public void AddBlog(Blog entity)
        {
            context.Blogs.Add(entity);
            context.SaveChanges();
        }

        public void DeleteBlog(int blogId)
        {
            var blog = context.Blogs.FirstOrDefault(p => p.Id == blogId);
            if (blog != null)
            {
                context.Blogs.Remove(blog);
                context.SaveChanges();
            }
        }

        public IQueryable<Blog> GetAll()
        {
            return context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            return context.Blogs.FirstOrDefault(p => p.Id == blogId);
        }

        public void SaveBlog(Blog entity)
        {
            //gelen entity id eğer 0 ise demek ki yeni blog kaydı yapılacak
            if (entity.Id == 0)
            {
                entity.Date = DateTime.Now; 
                context.Blogs.Add(entity);
            }
            //gelen id 0 dan farklı bir değer ise güncelleme yapılır
            else
            {
                var blog = GetById(entity.Id);
                if (blog != null)
                {
                    blog.Title = entity.Title;
                    blog.Description = entity.Description;
                    blog.CategoryId = entity.CategoryId;
                    blog.Image = entity.Image;
                    blog.Date = DateTime.Now;
                    blog.isHome = entity.isHome;
                    blog.isApproved = entity.isApproved;
                }
            }
            //eklenene veya güncellenen kayıt kaydedilir
            context.SaveChanges();
        }

        public void UpdateBlog(Blog entity)
        {
            //context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var blog = GetById(entity.Id);
            if (blog !=null)
            {
                blog.Title = entity.Title;
                blog.Description = entity.Description;
                blog.CategoryId = entity.CategoryId;
                blog.Image = entity.Image;
                blog.Date = DateTime.Now;
                blog.isHome = entity.isHome;
                blog.isApproved = entity.isApproved;
            }
            context.SaveChanges();
        }
    }
}
