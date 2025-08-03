using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project01_ApiDemo.Context;
using NetCoreAI.Project01_ApiDemo.Entities;

namespace NetCoreAI.Project01_ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomerController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CustomerList()
        {
            var value = _context.Customers.ToList();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("musteri ekleme islemi basarili");
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            if (value != null)
            {
                _context.Customers.Remove(value);
                _context.SaveChanges();
                return Ok("musteri silme islemi basarili");
            }
            return NotFound("musteri bulunamadi");
        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            if (value != null)
            {
                return Ok(value);
            }
            return NotFound("musteri bulunamadi");
        }

        [HttpPut]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Müşteri başarıyla güncellendi");
        }

    }
}
