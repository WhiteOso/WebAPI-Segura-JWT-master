using Newtonsoft.Json;
using System;

namespace ClientLibrary.Models
{
  public class UsuarioLogin
  {
    [JsonProperty("Username", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string Usuario { get; set; }

    [JsonProperty("Password", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public string Password { get; set; }
  }

}
