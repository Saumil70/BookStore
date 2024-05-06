using BookStore.DataAccess.Repository.IRepository;
using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models.ViewModel;
using System.Security.Cryptography.Xml;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
/*    [Authorize(Roles = SD.Role_Admin)]*/
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if(id==null|| id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitofwork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            

            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)

        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "the displayorder cannot exactly match with name");
            }*/
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if(file!= null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath,fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\Images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitofwork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitofwork.Product.Update(productVM.Product);
                }
                
                _unitofwork.Save();
                TempData["success"] = "Product addded successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {

                productVM.CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                   
                
                return View(productVM);
            }
           
        }

      
/*        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product Productfromdb = _unitofwork.Product.Get(u => u.Id == id);
            if (Productfromdb == null)
            {
                return NotFound();
            }
            return View(Productfromdb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)

        {

            Product Productfromdb = _unitofwork.Product.Get(u => u.Id == id);

            if (Productfromdb == null)
            {
                return NotFound();
            }

            _unitofwork.Product.Remove(Productfromdb);
            TempData["success"] = "Product deleted successfully";
            _unitofwork.Save();
            return RedirectToAction("Index", "Product");
        }*/

        #region API CALLS   

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }



        [HttpDelete]
        public IActionResult Delete(int? id) {
            var productToBeDeleted = _unitofwork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofwork.Product.Remove(productToBeDeleted);
            _unitofwork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
