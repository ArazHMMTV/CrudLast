using Business.Exceptions;
using Business.Extension;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
        }

        public async Task AddProduct(Product product)
        {
            if (product.ImageFile == null)
                throw new FileNullReferenceException("File bosdur!");

            product.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\products", product.ImageFile);

           await _productRepository.AddAsync(product);
           await _productRepository.CommitAsync();
        }

        public void DeleteProduct(int id)
        {
            var existProduct = _productRepository.Get(x=>x.Id == id);
            if (existProduct == null)
                throw new EntityNotFoundException("product yoxdur");

            Helper.DeleteFile(_env.WebRootPath, @"uploads\products", existProduct.ImageUrl);

            _productRepository.Delete(existProduct);
            _productRepository.Commit();
        }

        public List<Product> GetAllProducts(Func<Product, bool>? func = null)
        {
           return _productRepository.GetAll(func);
        }

        public Product GetProduct(Func<Product, bool>? func = null)
        {
            return _productRepository.Get(func);
        }

        public void UpdateProduct(int id, Product newProduct)
        {
            var oldProduct = _productRepository.Get(x=> x.Id == id);
            if (oldProduct == null)
                throw new EntityNotFoundException("product yoxdur");

            if (newProduct.ImageFile != null)
            {
                Helper.DeleteFile(_env.WebRootPath, @"uploads\products", oldProduct.ImageUrl);
                oldProduct.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\products", newProduct.ImageFile);
            }

            oldProduct.Name = newProduct.Name;
            oldProduct.Description = newProduct.Description;
            oldProduct.Price = newProduct.Price;
            _productRepository.Commit();
        }
    }
}
