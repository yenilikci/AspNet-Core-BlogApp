using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogrepo,ICategoryRepository categoryrepo)
        {
            _blogRepository = blogrepo;
            _categoryRepository = categoryrepo;
        }
            
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            //repository.GetAll() ile bir blog listesi gönderiyoruz
            return View(_blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            //blog kayıtın zamanına kayıt zamanı ataması
            entity.Date = DateTime.Now;

            if (ModelState.IsValid) //Validasyon gerekliliklerini geçmişse
            {
                _blogRepository.AddBlog(entity);
                return RedirectToAction("List");
            }
            return View(entity);
        }

    }
}
