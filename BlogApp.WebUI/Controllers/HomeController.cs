using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private IBlogRepository blogRepository;

        public HomeController(IBlogRepository repository)
        {
            blogRepository = repository;
        }

        public IActionResult Index()
        {
            return View(blogRepository.GetAll().Where(i => i.isApproved == true && i.isHome ==true));
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
