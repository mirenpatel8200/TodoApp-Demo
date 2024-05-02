using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RinkLine.Public.Helper
{
    public static class DataHelper<T> where T : class, new()
    {
        public async static Task<Response<T>> Execute(string baseUrl, string route, OperationType type, object payload = null)
        {
            Response<T> response = new Response<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("crossDomain", "true");

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                else if (type == OperationType.PUT)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PutAsync(route, stringContent);
                }
                else if (type == OperationType.DELETE)
                {
                    httpResponse = await client.DeleteAsync(route);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result.Data = JsonConvert.DeserializeObject<T>(result, new IsoDateTimeConverter());
                    response.Result.Success = true;
                }
                else
                {
                    response.Result.Data = JsonConvert.DeserializeObject<T>(result, new IsoDateTimeConverter());
                    //response.Message = "Something went wrong with api calling.";
                }
                return response;
            }
            catch (Exception ex)
            {
                //response.Message = "Error occured!!";
            }
            return response;
        }

        public async static Task<Response<T>> ExecuteWithToken(string baseUrl, string route, OperationType type, string token, object payload = null)
        {
            Response<T> response = new Response<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("crossDomain", "true");
                client.DefaultRequestHeaders.Add("Abp.TenantId", "1");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                else if (type == OperationType.PUT)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PutAsync(route, stringContent);
                }
                else if (type == OperationType.DELETE)
                {
                    httpResponse = await client.DeleteAsync(route);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                    //response.Message = "";
                }
                else
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                    //response.Message = "Something went wrong with api calling.";
                }
                return response;
            }
            catch (Exception ex)
            {
                //response.Message = "Error occured!!";
            }
            return response;
        }
    }

    public enum OperationType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class Response<T> where T : class
    {
        public Response()
        {
            Result = new Result<T>();
        }
        public Result<T> Result { get; set; }
    }

    public class Result<T> where T : class
    {
        [JsonProperty("result")]
        public T Data { get; set; }
        public string Message { get; set; }
        // public T Data { get; set; }

        [JsonProperty("targetUrl")]
        public object TargetUrl { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public Root Error { get; set; }

        [JsonProperty("unAuthorizedRequest")]
        public bool UnAuthorizeRequest { get; set; }

        [JsonProperty("__abp")]
        public bool __Abp { get; set; }
    }
    public class Root
    {
        public int code { get; set; }
        public string message { get; set; }
        public string details { get; set; }
        public object validationErrors { get; set; }
    }

    public class CustomRouteConfig
    {
        public static string GetDefaultPattern(string url)
        {
            if (url == "/")
            {
                return "{controller=Home}/{action=Login}/{id?}";
            }
            else if (url == "/Home/MemberLogin")
            {
                return "{controller=Home}/{action=MemberLogin}/{id?}";
            }
            else if (url == "/Login/Admin")
            {
                return "{controller=Login}/{action=Admin}/{id?}";
            }
            else
            {
                return "{controller=Home}/{action=Login}/{id?}";
            }
        }

        public static object GetDefaultRoute(string url)
        {
            if (url == "/")
            {
                return new { controller = "Home", action = "Login", area = "" };
            }
            else if (url == "/Home/MemberLogin")
            {
                return new { controller = "Home", action = "MemberLogin", area = "" };
            }
            else if (url == "/Login/Admin")
            {
                return new { controller = "Login", action = "Admin", area = "" };
            }
            else
            {
                return new { controller = "Home", action = "Login", area = "" };
            }
        }
    }
}
