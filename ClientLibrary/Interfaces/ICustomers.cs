using ClientLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Interfaces
{
  public interface ICustomers
  {
    string BaseUrl { get; set; }
    string BearerTokenJWT { get; set; }

    Task<Customer> DeleteCustomerAsync(Guid id, Customer customer);
    Task<Customer> GetCustomerAsync(Guid id);
    Task<List<Customer>> GetCustomersAsync();
    Task<Customer> PostCustomerAsync(Customer customer);
    Task<Customer> PutCustomerAsync(Guid id, Customer customer);
  }
}
