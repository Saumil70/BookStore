using BookStore_Razor.Data;
using BookStore_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore_Razor.Pages.Categories
{
    

        [BindProperties]
        public class EditModel : PageModel
        {
            private readonly BookStoreEntites _db;

            public Category Category { get; set; }

            public EditModel(BookStoreEntites db)
            {
                _db = db;
            }
            public void OnGet(int? id)
            {
                if(id!=null && id != 0)
                {
                Category = _db.Categories.Find(id);


                }
            }

            public IActionResult OnPost()
            {
                if(ModelState.IsValid)
                {
                _db.Categories.Update(Category);
                TempData["success"] = "Category Edited successfully";
                _db.SaveChanges();  
                return RedirectToPage("Index");
                }
                return Page();  
            }
        
    }
}
