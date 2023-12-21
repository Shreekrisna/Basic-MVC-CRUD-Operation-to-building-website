using Microsoft.AspNetCore.Mvc;
using VoluminousBook.DataAccess;
using VoluminousBook.DataAccess.Repository.IRepository;
using VoluminousBook.Models;

namespace VoluminousBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        
        private readonly ICategoryRepository _db;

        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objcategoryList = _db.GetAll();
            return View(objcategoryList);
        }
        //GET
        public IActionResult Create()
        {
            
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //help and prevent from crosssite request forgery attack
        public IActionResult Create(Category obj)
        {
            if(obj.Name==obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DiplayOrder cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);   
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
           // var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if(categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //help and prevent from crosssite request forgery attack
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DiplayOrder cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] //help and prevent from crosssite request forgery attack
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj==null)
            {
                return NotFound();
            }
            
            
            _db.Remove(obj);
            _db.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
            
            
        }
    }
}
