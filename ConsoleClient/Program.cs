using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//https://www.rafaelacosta.net/Blog/2019/10/29/c%C3%B3mo-crear-un-cliente-c-para-un-web-api-de-aspnet-core-iii

namespace ConsoleClient
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Object obj;
      Console.WriteLine("Press key to continue");
      Console.ReadKey();
      Login("echoping");
      Login("echouser");
      //PostItem("authenticate","admin","123456");

      HttpClient httpClient;
      ClientLibrary.LoginClient login;
      ClientLibrary.Models.UsuarioLogin usuarioLogin;
      ClientLibrary.CustomerClient customer;

      httpClient = new HttpClient();
      usuarioLogin = new ClientLibrary.Models.UsuarioLogin();
      login = new ClientLibrary.LoginClient(httpClient);
      customer = new ClientLibrary.CustomerClient(httpClient);

      usuarioLogin.Usuario = "admin";
      usuarioLogin.Password = "123456";

      login.BaseUrl = "httP://localhost:49220";

      Console.WriteLine("-->send LoginAsync");
      obj = await login.LoginAsync(usuarioLogin);
      Console.WriteLine($"<--received [{obj}]");
      //Task<object> task = _login.LoginAsync(usuarioLogin);
      //obj = task.Result;

      customer.BaseUrl = "httP://localhost:49220";
      customer.BearerTokenJWT = (string)obj;


      Console.WriteLine("-->send GetCustomersAsync");
      obj = await customer.GetCustomersAsync();
      var json = JsonConvert.SerializeObject(obj);
      Console.WriteLine($"<--received [{json}]");

      Console.WriteLine("-->send LoginAsync");
      Guid guid = Guid.NewGuid();
      obj = await customer.GetCustomerAsync(guid);
      json = JsonConvert.SerializeObject(obj);
      Console.WriteLine($"<--received [{json}]");

      Console.ReadKey();
    }

    private static void PostItem(string method, string user, string pass)
    {
      //var url = $"http://localhost:49220/api/login/{method}";
      //var request = (HttpWebRequest)WebRequest.Create(url);
      //string json = Newtonsoft.Json$"{{\"username\":\"{user}\"}}, {{\"Password\":\"{pass}\"}}";
      //request.Method = "POST";
      //request.ContentType = "application/json";
      //request.Accept = "application/json";

      //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
      //{
      //  streamWriter.Write(json);
      //  streamWriter.Flush();
      //  streamWriter.Close();
      //}

      //try
      //{
      //  using (WebResponse response = request.GetResponse())
      //  {
      //    using (Stream strReader = response.GetResponseStream())
      //    {
      //      if (strReader == null) return;
      //      using (StreamReader objReader = new StreamReader(strReader))
      //      {
      //        string responseBody = objReader.ReadToEnd();
      //        // Do something with responseBody
      //        Console.WriteLine(responseBody);
      //      }
      //    }
      //  }
      //}
      //catch (WebException ex)
      //{
      //  // Handle error
      //}
    }

    private static void Login(string method)
    {
      var url = $"http://localhost:49220/api/login/{method}";
      var request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = "GET";
      request.ContentType = "application/json";
      request.Accept = "application/json";

      Console.WriteLine($"send {method}");
      try
      {
        using (WebResponse response = request.GetResponse())
        {
          using (Stream strReader = response.GetResponseStream())
          {
            if (strReader == null) return;
            using (StreamReader objReader = new StreamReader(strReader))
            {
              string responseBody = objReader.ReadToEnd();
              // Do something with responseBody
              Console.WriteLine($"received { responseBody}");
            }
          }
        }
      }
      catch (WebException ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
