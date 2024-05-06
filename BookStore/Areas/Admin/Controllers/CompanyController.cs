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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork db)
        {
            _unitofwork = db;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {


            if(id==null|| id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company CompanyObj = _unitofwork.Company.Get(u => u.Id == id);
                return View(CompanyObj);
            }
            

            
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)

        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "the displayorder cannot exactly match with name");
            }*/
            if (ModelState.IsValid)
            {
              

                if (CompanyObj.Id == 0)
                {
                    _unitofwork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitofwork.Company.Update(CompanyObj);
                }
                
                _unitofwork.Save();
                TempData["success"] = "Company addded successfully";
                return RedirectToAction("Index", "Company");
            }
            else
            {

                              
                return View(CompanyObj);
            }
           
        }

      
/*        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Company Companyfromdb = _unitofwork.Company.Get(u => u.Id == id);
            if (Companyfromdb == null)
            {
                return NotFound();
            }
            return View(Companyfromdb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)

        {

            Company Companyfromdb = _unitofwork.Company.Get(u => u.Id == id);

            if (Companyfromdb == null)
            {
                return NotFound();
            }

            _unitofwork.Company.Remove(Companyfromdb);
            TempData["success"] = "Company deleted successfully";
            _unitofwork.Save();
            return RedirectToAction("Index", "Company");
        }*/

        #region API CALLS   

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
            return Json(new {data = objCompanyList});
        }



        [HttpDelete]
        public IActionResult Delete(int? id) {
            var CompanyToBeDeleted = _unitofwork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitofwork.Company.Remove(CompanyToBeDeleted);
            _unitofwork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
