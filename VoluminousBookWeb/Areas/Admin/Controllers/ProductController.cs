using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VoluminousBook.DataAccess;
using VoluminousBook.DataAccess.Repository.IRepository;
using VoluminousBook.Models;
using VoluminousBook.Models.ViewModels;

namespace VoluminousBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;  
        }

        public IActionResult Index()
        {
            
            return View();
        }


        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                })

        };
            if (id == null || id == 0)
            {
                //Create Product
               // ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //Update Product
            }
           
            return View(productVM);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //help and prevent from crosssite request forgery attack
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {

          
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null) //This means file is uploaded
                {
                    string fileName = Guid.NewGuid().ToString();//created a new filename
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);//like whether it is .txt or.png file

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products" + fileName + extension;

                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "product Created Successfully";
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
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //help and prevent from crosssite request forgery attack
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType Deleted Successfully";
            return RedirectToAction("Index");


        }

        //Creating built in api endpoints
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperies:"Category,CoverType");
            return Json(new {data=productList});
        }
        #endregion
    }
}
