using BookStore_Razor.Data;
using BookStore_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore_Razor.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly BookStoreEntites _db;

        public Category Category { get; set; }

        public DeleteModel(BookStoreEntites db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = _db.Categories.Find(id);


            }
        }

        public IActionResult OnPost()
        {
            Category obj = _db.Categories.Find(Category.Id);
            if(obj== null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            TempData["success"] = "Category deleted successfully";
            _db.SaveChanges();
            return RedirectToPage("Index");
        }

    }
}
