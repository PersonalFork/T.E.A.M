using log4net;

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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RestClient));

        public static async Task<ResponseDto<T>> GetDelete<T>(string getDeleteUri,
            MethodType methodType, string authorizationToken = null)
        {
            _logger.Info(string.Format("Method {0} {1}", methodType.ToString(), getDeleteUri));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(authorizationToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);
                }
                HttpResponseMessage response = null;
                try
                {
                    switch (methodType)
                    {
                        case MethodType.DELETE:
                            response = await client.DeleteAsync(getDeleteUri);
                            break;
                        case MethodType.GET:
                            response = await client.GetAsync(getDeleteUri);
                            break;
                        case MethodType.PUT:
                        case MethodType.POST:
                        default:
                            throw new Exception("Put/Post is not supported by this method.");
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Error(string.Format("Method Failed : {0} {1} , Reason : {2}",
                            methodType.ToString(), getDeleteUri, response.ReasonPhrase));
                        return new ResponseDto<T>()
                        {
                            Data = default(T),
                            IsSuccess = false,
                            StatusCode = response.StatusCode,
                            ErrorMessage = response.ReasonPhrase
                        };
                    }

                    T result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    return new ResponseDto<T>()
                    {
                        Data = result,
                        IsSuccess = true,
                        StatusCode = response.StatusCode
                    };
                }
                catch (HttpRequestException ex)
                {
                    _logger.Error(string.Format("Method Failed : {0} {1}", methodType.ToString(), getDeleteUri), ex);

                    HttpStatusCode statusCode = default(HttpStatusCode);
                    if (response != null)
                    {
                        statusCode = response.StatusCode;
                    }
                    return new ResponseDto<T>()
                    {
                        Data = default(T),
                        ErrorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                        IsSuccess = false,
                        StatusCode = statusCode
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Method Failed : {0} {1}", methodType.ToString(), getDeleteUri), ex);

                    HttpStatusCode statusCode = default(HttpStatusCode);
                    if (response != null)
                    {
                        statusCode = response.StatusCode;
                    }
                    return new ResponseDto<T>()
                    {
                        Data = default(T),
                        ErrorMessage = ex.Message,
                        IsSuccess = false,
                        StatusCode = statusCode
                    };
                }
            }
        }

        public static async Task<ResponseDto<T>> PutPost<T, U>(string putPostUri, MethodType methodType, U data,
            string authorizationToken = null, Dictionary<string, string> headers = null, bool serialize = true)
        {
            _logger.Info(string.Format("Method {0} {1}", methodType.ToString(), putPostUri));
            using (HttpClient client = new HttpClient())
            {
                string json = string.Empty;
                // additional control to put/post without deserialization.
                if (serialize)
                {
                    json = JsonConvert.SerializeObject(data);
                }
                else
                {
                    if (data is string)
                    {
                        json = data.ToString();
                    }
                }

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                if (!string.IsNullOrEmpty(authorizationToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);
                }

                HttpResponseMessage response = null;
                try
                {
                    // add headers.
                    if (headers != null && headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    switch (methodType)
                    {
                        case MethodType.PUT:
                            response = await client.PutAsync(putPostUri, content);
                            break;

                        case MethodType.POST:
                            response = await client.PostAsync(putPostUri, content);
                            break;

                        case MethodType.DELETE:
                        case MethodType.GET:
                        default:
                            throw new Exception("Get/Delete is not supported by this method.");
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Error(string.Format("Method Failed : {0} {1} {2}, Reason : ",
                           methodType.ToString(), putPostUri, response.ReasonPhrase));

                        return new ResponseDto<T>()
                        {
                            Data = default(T),
                            IsSuccess = false,
                            StatusCode = response.StatusCode,
                            ErrorMessage = response.ReasonPhrase
                        };
                    }

                    T result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    return new ResponseDto<T>()
                    {
                        Data = result,
                        IsSuccess = true,
                        StatusCode = response.StatusCode
                    };
                }
                catch (HttpRequestException ex)
                {
                    _logger.Error(string.Format("Method Failed : {0} {1}", methodType.ToString(), putPostUri), ex);

                    HttpStatusCode statusCode = default(HttpStatusCode);
                    if (response != null)
                    {
                        statusCode = response.StatusCode;
                    }
                    return new ResponseDto<T>()
                    {
                        Data = default(T),
                        ErrorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                        IsSuccess = false,
                        StatusCode = statusCode
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Method Failed : {0} {1}", methodType.ToString(), putPostUri), ex);

                    HttpStatusCode statusCode = default(HttpStatusCode);
                    if (response != null)
                    {
                        statusCode = response.StatusCode;
                    }
                    return new ResponseDto<T>()
                    {
                        Data = default(T),
                        ErrorMessage = ex.Message,
                        IsSuccess = false,
                        StatusCode = statusCode
                    };
                }
            }
        }

        public static async Task<ResponseDto<T>> PutPost<T, U>(string baseUri, string methodName, MethodType methodType, U data, string authorizationToken)
        {
            string url = string.Join("/", baseUri, methodName);
            ResponseDto<T> ret = await PutPost<T, U>(url, methodType, data, authorizationToken);

            return ret;
        }

        public static async Task<ResponseDto<T>> GetDelete<T>(string baseUri, MethodType methodType, bool prependQMark, Dictionary<string, string> parameters = null, string authorizationToken = null)
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
            ResponseDto<T> ret = await GetDelete<T>(baseUri, methodType, authorizationToken);
            return ret;
        }
    }
}
