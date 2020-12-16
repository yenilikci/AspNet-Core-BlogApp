using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private ICategoryRepository _repository;

        public CategoryMenuViewComponent(ICategoryRepository repo)
        {
            _repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            //seçilen kategorinin id'si alınıp daha sonra active class'ı verilecek, RouteData nullable
            ViewBag.SelectedCategory = RouteData?.Values["id"];
            //bütün kategori bilgilerini view üzerine taşıdık
            return View(_repository.GetAll());
        }
    }
}
