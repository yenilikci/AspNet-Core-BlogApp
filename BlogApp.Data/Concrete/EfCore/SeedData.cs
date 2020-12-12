﻿using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            BlogContext context = app.ApplicationServices.GetRequiredService<BlogContext>();
            context.Database.Migrate();

            if (!context.Categories.Any()) //eğer herhangi bir kategori yok ise 
            {
                context.Categories.AddRange(
                    new Category() { Name="Category1"},
                    new Category() { Name = "Category2" },
                    new Category() { Name = "Category3" }
                    );

                context.SaveChanges(); //yukarıdaki kayıtları ekler
            }


            if (!context.Blogs.Any()) //eğer herhangi bir blog yazısı yok ise
            {
                context.Blogs.AddRange(
                    new Blog() { Title="Blog Title 1 ",Description= "Blog Description", Body="Blog Body 1",Image="1.jpg",Date=DateTime.Now.AddDays(-5),isApproved=true,CategoryId=1},
                    new Blog() { Title = "Blog Title 2 ", Description = "Blog Description", Body = "Blog Body 2", Image = "2.jpg", Date = DateTime.Now.AddDays(-5), isApproved = true, CategoryId = 2 },
                    new Blog() { Title = "Blog Title 3 ", Description = "Blog Description", Body = "Blog Body 3", Image = "3.jpg", Date = DateTime.Now.AddDays(-5), isApproved = true, CategoryId = 3 },
                    new Blog() { Title = "Blog Title 4 ", Description = "Blog Description", Body = "Blog Body 4", Image = "4.jpg", Date = DateTime.Now.AddDays(-5), isApproved = true, CategoryId = 2 }
                    );

                context.SaveChanges(); //yukarıdaki kayıtları ekler
            }

        }
    }
}
