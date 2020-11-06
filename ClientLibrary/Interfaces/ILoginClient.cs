using ClientLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Interfaces
{
  public interface ILoginClient
  {
    string BaseUrl { get; set; }

    Task<object> LoginAsync(UsuarioLogin usuarioLogin);
  }
}
