using ClientLibrary.Interfaces;
using ClientLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary
{
  public class CustomerClient : ICustomers
  {
    //////////////////////////////////////
    // DEFINICIÓN DE VARIABLES PRIVADAS //
    //////////////////////////////////////
    private string _baseUrl = "https://localhost:5001";

    // DEFINIMOS EL TOKEN JWT A NIVEL DE CLASE.
    private string _bearerTokenJWT = string.Empty;

    // DEFINIMOS EL CLIENTE HTTP A NIVELDE CLASE.
    private HttpClient _httpClient;

    // OBTENEMOS A TRAVÉS DE CONSTRUCTOR EL HttpClient
    // (POR INYECCIÓN DE DEPENDENCIAS).
    public CustomerClient(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    // PROPIEDAD PUBLICA PARA ASIGNAR O LEER
    // LA URL-BASE DEL WEB-API.
    public string BaseUrl
    {
      get { return _baseUrl; }
      set { _baseUrl = value; }
    }

    // PROPIEDAD PUBLICA PARA ASIGNAR O LEER
    // EL TOKEN JWT DE ACCESO
    public string BearerTokenJWT
    {
      get { return _bearerTokenJWT; }
      set { _bearerTokenJWT = value; }
    }

    //////////////////////////////////////
    // ACCIONES/OPERACIONES DEL WEB-API //
    //////////////////////////////////////
    public Task<Customer> DeleteCustomerAsync(Guid id, Customer customer)
    {
      throw new NotImplementedException();
    }

    public async Task<Customer> GetCustomerAsync(Guid id)
    {
      if (id == null)
        throw new System.ArgumentNullException("id");

      // CONSTRUIMOS LA URL //
      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
                 .Append("/api/customers/{id}");
      urlBuilder_.Replace("{id}", Uri.EscapeDataString(id.ToString()));
      var url_ = urlBuilder_.ToString();

      // RECUPERAMOS EL HttpClient
      var client_ = _httpClient;

      try
      {
        using (var request_ = new HttpRequestMessage())
        {
          // CONSTRUIMOS LA PETICIÓN (REQUEST) //
          request_.Method = new HttpMethod("GET");
          request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);
          request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
          if (!string.IsNullOrEmpty(_bearerTokenJWT))
            request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerTokenJWT);

          // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
          var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
          var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

          // RESPUESTA ESPERADA !! //
          if (response_.StatusCode == System.Net.HttpStatusCode.OK) // 200
          {
            var responseBody_ = JsonConvert.DeserializeObject<Customer>(responseText_);
            return responseBody_;
          }
          else
          if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
          {
            throw new Exception("401 Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso. " +
                responseText_);
          }
          else
          if (response_.StatusCode == System.Net.HttpStatusCode.NotFound) // 404
          {
            throw new Exception("404 NotFound. No se ha encontrado el objeto solicitado. " +
                responseText_);
          }
          else
          if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
              response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
          {
            throw new Exception((int)response_.StatusCode + ". No se esperaba el código de estado HTTP de la respuesta. " +
                responseText_);
          }
          return default(Customer);
        }
      }
      finally
      {
        // NO UTILIZAMOS CATCH, 
        // PASAMOS LA EXCEPCIÓN A LA APP.
      }
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
      // CONSTRUIMOS LA URL DE LA ACCIÓN
      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
                 .Append("/api/customers");
      var url_ = urlBuilder_.ToString();

      // RECUPERAMOS EL HttpClient
      var client_ = _httpClient;

      try
      {
        using (var request_ = new HttpRequestMessage())
        {
          ///////////////////////////////////////
          // CONSTRUIMOS LA PETICIÓN (REQUEST) //
          ///////////////////////////////////////
          // DEFINIMOS EL MÉTODO HTTP
          request_.Method = new HttpMethod("GET");

          // DEFINIMOS LA URI
          request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

          // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
          request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

          // ASIGNAMOS A LA CABECERA DE LA PETICIÓN EL TOKEN JWT.
          if (!string.IsNullOrEmpty(_bearerTokenJWT))
            request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerTokenJWT);

          /////////////////////////////////////////
          // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
          /////////////////////////////////////////
          // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
          var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

          // OBTENEMOS EL Content DEL RESPONSE como un String
          // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
          var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

          // SI ES LA RESPUESTA ESPERADA !! ...
          if (response_.StatusCode == System.Net.HttpStatusCode.OK) // 200
          {
            // DESERIALIZAMOS Content DEL RESPONSE
            var responseBody_ = JsonConvert.DeserializeObject<List<Customer>>(responseText_);
            return responseBody_;
          }
          else
          // SI NO SE ESTÁ AUTORIZADO ...
          if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
          {
            throw new Exception("401 Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso. " +
                responseText_);
          }
          else
          // CUALQUIER OTRA RESPUESTA ...
          if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
              response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
          {
            throw new Exception((int)response_.StatusCode + ". No se esperaba el código de estado HTTP de la respuesta. " +
                responseText_);
          }

          // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
          return default(List<Customer>);
        }
      }
      finally
      {
        // NO UTILIZAMOS CATCH, 
        // PASAMOS LA EXCEPCIÓN A LA APP.
      }
    }

    public Task<Customer> PostCustomerAsync(Customer customer)
    {
      throw new NotImplementedException();
    }

    public Task<Customer> PutCustomerAsync(Guid id, Customer customer)
    {
      throw new NotImplementedException();
    }
  }
}
