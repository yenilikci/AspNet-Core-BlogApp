using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;

        public CategoryController(ICategoryRepository _repo)
        {
            repository = _repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(repository.GetAll());
        }
      
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.AddCategory(entity);
                return RedirectToAction("List");
            }
            return View(entity);
        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            //ekleme işlemi
            if (id == null)
            {
                return View(new Category());
            }    
            else //güncelleme işlemi
            {
                return View(repository.GetById((int)id));
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCategory(entity);
                TempData["message"] = $"{entity.Name} kayıt edildi";
                return RedirectToAction("List");
            }
            //eğer bir problem varsa
            return View(entity);
        }


    }

}
