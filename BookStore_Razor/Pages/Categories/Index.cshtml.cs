using BookStore_Razor.Data;
using BookStore_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore_Razor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly BookStoreEntites _db;
        public List<Category> CategoryList { get; set; }  
        
        public IndexModel(BookStoreEntites db)
        {
            _db = db;   
        }
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
