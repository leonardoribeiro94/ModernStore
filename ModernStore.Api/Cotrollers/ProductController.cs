using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.Repositories;
using System;
using Microsoft.AspNetCore.Authorization;

namespace ModernStore.Api.Cotrollers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        [Route("v1/products")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(_productRepository.Get());
        }

        [HttpGet]
        [Route("v1/products/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok($"Produto {id}");
        }

        [HttpPost]
        [Route("v1/products")]
        public IActionResult Post()
        {
            return Ok("Criando um novo produto");
        }
    }
}
