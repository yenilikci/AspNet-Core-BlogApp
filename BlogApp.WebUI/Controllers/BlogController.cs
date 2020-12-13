using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository repository;

        public BlogController(IBlogRepository _repo)
        {
            repository = _repo;
        }
            
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            //repository.GetAll() ile bir blog listesi gönderiyoruz
            return View(repository.GetAll());
        }

    }
}
