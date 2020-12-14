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
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name"); 
            return View(entity);
        }
           
        [HttpGet]
        public IActionResult Edit(int id) //dışarıdan bir id almalı gönderdiğim id'ye göre sorgulama yapıp veritabından seçtiğim veriyi form üzerine gönderebilmeliyim
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(_blogRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Blog entity)
        {
            if (ModelState.IsValid)
            {
                _blogRepository.UpdateBlog(entity);
                //ekleme işlemi yapılır List view'ine kullanıcı yönlendirilir, mesajda gönderilir
                TempData["message"] = $"{entity.Title} güncellendi";
                return RedirectToAction("List");
            }
            //eğer bir problem varsa problemli entity'i kullanıcıya gösterelim
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(entity);

        }

    }
}
