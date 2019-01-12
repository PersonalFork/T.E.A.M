using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TEAM.Common.Rest
{
    public enum MethodType
    {
        GET,
        PUT,
        POST,
        DELETE
    }
    public class RestClient
    {
        public static async Task<ResponseDto<T>> GetDelete<T>(string url, MethodType methodType, Dictionary<string, KeyValuePair<string, string>> requestHeaders = null)
        {
            HttpClientHandler authtHandler = new HttpClientHandler()
            {
                // Credentials = CredentialCache.DefaultNetworkCredentials
                Credentials = new NetworkCredential("jchakraborty", "jchakraborty", "ids")
            };
            using (HttpClient client = new HttpClient(authtHandler))
            {
                // add request headers.
                //if (requestHeaders != null && requestHeaders.ContainsKey("Authorization"))
                //{
                //    KeyValuePair<string, string> header = requestHeaders["Authorization"];
                //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Key, header.Value);
                //}
                if (requestHeaders == null || !requestHeaders.ContainsKey("Accept"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else if (requestHeaders != null && !requestHeaders.ContainsKey("Accept"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    KeyValuePair<string, string> header = requestHeaders["Accept"];
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                }

                // process get/delete request.
                HttpResponseMessage response = null;
                try
                {
                    switch (methodType)
                    {
                        case MethodType.PUT:
                        case MethodType.POST:
                            throw new Exception("Get/Delete is not supported by this method.");

                        case MethodType.DELETE:
                            response = await client.DeleteAsync(url);
                            break;
                        case MethodType.GET:
                            response = await client.GetAsync(url);
                            break;
                        default:
                            throw new Exception("Get/Delete is not supported by this method.");
                    }

                    T result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    return new ResponseDto<T>()
                    {
                        Data = result,
                        IsSuccess = true,
                        StatusCode = response.StatusCode
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseDto<T>()
                    {
                        ErrorMessage = ex.Message,
                        IsSuccess = false,
                        StatusCode = response.StatusCode
                    };
                }
            }
        }

        public static async Task<ResponseDto<T>> PutPost<T, U>(string postUri, MethodType methodType, U data, Dictionary<string, KeyValuePair<string, string>> requestHeaders = null)
        {
            using (HttpClient client = new HttpClient())
            {
                // add request headers.
                if (requestHeaders != null && requestHeaders.ContainsKey("Authorization"))
                {
                    KeyValuePair<string, string> header = requestHeaders["Authorization"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Key, header.Value);
                }

                // process get/put request.
                string json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    switch (methodType)
                    {
                        case MethodType.PUT:
                            response = await client.PutAsync(postUri, content);
                            break;

                        case MethodType.POST:
                            response = await client.PostAsync(postUri, content);
                            break;

                        case MethodType.DELETE:
                        case MethodType.GET:
                        default:
                            throw new Exception("Get/Delete is not supported by this method.");
                    }

                    T result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    return new ResponseDto<T>()
                    {
                        Data = result,
                        IsSuccess = true,
                        StatusCode = response.StatusCode
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseDto<T>()
                    {
                        ErrorMessage = ex.Message,
                        IsSuccess = false,
                        StatusCode = response.StatusCode
                    };
                }
            }
        }

        public static async Task<ResponseDto<T>> PutPost<T, U>(string baseUri, string methodName, MethodType methodType, U data, Dictionary<string, KeyValuePair<string, string>> requestHeaders = null)
        {
            string url = string.Join("/", baseUri, methodName);
            ResponseDto<T> ret = await PutPost<T, U>(url, methodType, data, requestHeaders);

            return ret;
        }

        public static async Task<ResponseDto<T>> GetDelete<T>(string baseUri, MethodType methodType, bool prependQMark, Dictionary<string, string> parameters = null, Dictionary<string, KeyValuePair<string, string>> requestHeaders = null)
        {
            StringBuilder query = new StringBuilder();
            if (parameters != null && parameters.Any())
            {
                if (string.IsNullOrEmpty(query.ToString()))
                {
                    query.Append("?");
                }
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    if (prependQMark && !query.ToString().EndsWith("?") && parameters.Count > 1)
                    {
                        query.Append("&");
                    }
                    if (!(string.IsNullOrEmpty(parameter.Key) || string.IsNullOrEmpty(parameter.Value)))
                    {
                        string key = parameter.Key.Replace(" ", "");
                        string value = parameter.Value.Replace(" ", "+");
                        query.Append(key);
                        query.Append("=");
                        query.Append(value);
                    }
                }
            }
            baseUri = baseUri + query;
            ResponseDto<T> ret = await GetDelete<T>(baseUri, methodType, requestHeaders);
            return ret;
        }
    }
}
