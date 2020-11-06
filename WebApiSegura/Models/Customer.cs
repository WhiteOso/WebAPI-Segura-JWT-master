using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
  public class Customer
  {

    public Customer()
    {
      this.Id = Guid.NewGuid();
    }
    [JsonProperty("Id", Required = Newtonsoft.Json.Required.AllowNull)]
    public Guid Id { get; set; }

    //[Required]
    //[StringLength(50, MinimumLength = 3)]
    [JsonProperty("Nombre", Required = Newtonsoft.Json.Required.AllowNull)]
    public string Nombre { get; set; }

    //[Required]
    [JsonProperty("Numero", Required = Newtonsoft.Json.Required.AllowNull)]
    public int Numero { get; set; }
  }
}