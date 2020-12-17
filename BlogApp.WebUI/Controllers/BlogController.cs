using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogrepo, ICategoryRepository categoryrepo)
        {
            _blogRepository = blogrepo;
            _categoryRepository = categoryrepo;
        }

        public IActionResult Index(int? id,string q)
        {
                var query = _blogRepository.GetAll().Where(i => i.isApproved == true);
                if (id != null)
                {
                    query = query.Where(i => i.CategoryId == id);
                }

                if (!string.IsNullOrEmpty(q)) //burdan gelen q null ya da boşluk karakterine eşit değil ise
                {
                query = query.Where(i => i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q)); //3 farklı yerde arama yapıyoruz bunlardan herhangi biri true dönerse ilgili olan kayıt gelir
                }

                return View(query.OrderByDescending(i => i.Date));   
        }

        public IActionResult Details(int id)
        {
            return View(_blogRepository.GetById(id));
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
        public async Task<IActionResult> Edit(Blog entity,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    //kayıt resmi için
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        //tabi entity Image'i de kaydedelim
                        entity.Image = file.FileName;
                    }
                }

                _blogRepository.UpdateBlog(entity);
                //ekleme işlemi yapılır List view'ine kullanıcı yönlendirilir, mesajda gönderilir
                TempData["message"] = $"{entity.Title} güncellendi";
                return RedirectToAction("List");
            }
            //eğer bir problem varsa problemli entity'i kullanıcıya gösterelim
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(entity);

        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id) //id nullable çünkü yeni bir kayıt yaparsam buraya id gelmeyecek güncelleme işlemi ise id olacak
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");

            if (id == null)
            {
                //yeni kayıt  
                return View(new Blog()); //içerisinde yeni blog elemanı göndermeliyiz (Id oto 0 atanır)
            }
            else
            {
                //güncelleme
                return View(_blogRepository.GetById((int)id)); //id nullable olduğu için int cast edildi
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {
            if (ModelState.IsValid)
            {
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi";
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(entity);
        }

        [HttpGet]
        public IActionResult Delete(int id) //list'den delete butonuna basılınca id ile gelir
        {
            return View(_blogRepository.GetById(id));
            //gelen id ile kayıtı alır form üzerine aktarırız
        }


        [HttpPost,ActionName("Delete")] //get ve post actionların imzası aynı olacağından hatanın önüne geçmek için post kısmını DeleteConfirmed yaparsak
        //ve ActionName("Delete") diye parametre olarak verirsek hatayı önleriz
        public IActionResult DeleteConfirmed(int id) 
        {
            _blogRepository.DeleteBlog(id);
            TempData["message"] = $"{id} numaralı kayıt silindi";
            return RedirectToAction("List");
        }

    }
}
