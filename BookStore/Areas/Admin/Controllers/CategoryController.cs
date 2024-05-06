using BookStore.DataAccess.Repository.IRepository;
using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookStore.Utility;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
/*    [Authorize(Roles=SD.Role_Admin)]*/
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public CategoryController(IUnitOfWork db)
        {
            _unitofwork = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitofwork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)

        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "the displayorder cannot exactly match with name");
            }*/
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(obj);
                _unitofwork.Save();
                TempData["success"] = "Category addded successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryfromdb = _unitofwork.Category.Get(u => u.Id == id);
            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)

        {

            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(obj);
                _unitofwork.Save();
                TempData["success"] = "Category update successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryfromdb = _unitofwork.Category.Get(u => u.Id == id);
            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)

        {

            Category categoryfromdb = _unitofwork.Category.Get(u => u.Id == id);

            if (categoryfromdb == null)
            {
                return NotFound();
            }

            _unitofwork.Category.Remove(categoryfromdb);
            TempData["success"] = "Category deleted successfully";
            _unitofwork.Save();
            return RedirectToAction("Index", "Category");
        }
    }
}
