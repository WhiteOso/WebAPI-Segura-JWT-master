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
  public class LoginClient : ILoginClient
  {
    // DEFINICIÓN DE VARIABLES PRIVADAS.
    private string _baseUrl = "httP://localhost:49220";

    private HttpClient _httpClient;

    // OBTENEMOS A TRAVÉS DE CONSTRUCTOR EL HttpClient.
    public LoginClient(HttpClient httpClient)
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

    //////////////////////////////////////
    // ACCIONES/OPERACIONES DEL WEB-API //
    //////////////////////////////////////

    /////////////////
    // ACCIÓN POST //
    /////////////////
    public async Task<object> LoginAsync(UsuarioLogin usuarioLogin)
    {
      // CONSTRUIMOS LA URL DE LA ACCIÓN
      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
                 .Append("/api/login/authenticate");
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
          // DEFINIMOS EL Content CON EL OBJETO A ENVIAR SERIALIZADO.
          request_.Content = new StringContent(JsonConvert.SerializeObject(usuarioLogin));

          // DEFINIMOS EL ContentType, EN ESTE CASO ES "application/json"
          request_.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

          // DEFINIMOS EL MÉTODO HTTP
          request_.Method = new HttpMethod("POST");

          // DEFINIMOS LA URI
          request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

          // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
          request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

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
            var responseBody_ = JsonConvert.DeserializeObject<object>(responseText_);
            return responseBody_;
          }
          else
          // SI NO SE ESTÁ AUTORIZADO ...
          if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
          {
            throw new Exception("401 Unauthorized. Las credenciales de acceso del usuario son incorrectas. " +
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
          return default(object);
        }
      }
      finally
      {
        // NO UTILIZAMOS CATCH, 
        // PASAMOS LA EXCEPCIÓN A LA APP.
      }
    }
  }
}
