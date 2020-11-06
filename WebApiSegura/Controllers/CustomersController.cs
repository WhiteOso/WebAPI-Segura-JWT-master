using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
  /// <summary>
  /// customer controller class for testing security token 
  /// </summary>
  [Authorize]
  [RoutePrefix("api/customers")]
  public class CustomersController : ApiController
  {
    [HttpGet]
    public IHttpActionResult GetId(Guid id)
    {
      Random random = new Random();
      int randomNumber = random.Next(0, 1000);
      var customerFake = new Customer() { Id= id, Nombre = $"customer{randomNumber}", Numero = randomNumber };
      return Ok(customerFake);
    }

    [HttpGet]
    public IHttpActionResult GetAll()
    {
      List<Customer> customers;
      customers = new List<Customer>();

      customers.Add(new Customer { Nombre = "customer1", Numero = 1 });
      customers.Add(new Customer { Nombre = "customer2", Numero = 2 });
      customers.Add(new Customer { Nombre = "customer3", Numero = 3 });

      //var customersFake = new string[] { "customer-1", "customer-2", "customer-3" };
      var customersFake = customers.ToList();
      return Ok(customersFake);
    }
  }
}
