using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Giftos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Create(Product product)
        {
            if(!ModelState.IsValid)
                return View();

            try
            {
                await _productService.AddProduct(product);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("ImageFile" , ex.Message); 
            }
            catch(ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile",ex.Message);   
            }
            catch (FileNullReferenceException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
          
        }
        public IActionResult Delete(int id) 
        {
           var existProduct = _productService.GetProduct(x=>x.Id == id);
            if (existProduct == null) return NotFound();
            return View(existProduct);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");   
        }
        public IActionResult Update(int id)
        {
            var oldProduct = _productService.GetProduct(x=>x.Id==id);
            if (oldProduct == null) return NotFound();
            return View(oldProduct);
        }
        [HttpPost]
        public IActionResult Update(Product newProduct)
        {
            if(!ModelState.IsValid)
                return View();
            try
            {
                _productService.UpdateProduct(newProduct.Id, newProduct);

            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
