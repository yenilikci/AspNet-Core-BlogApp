using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext : DbContext
    {
        //kurucu oluşturup connection string
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }

        //entitylerden DbSetlerimi oluşturdum
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
