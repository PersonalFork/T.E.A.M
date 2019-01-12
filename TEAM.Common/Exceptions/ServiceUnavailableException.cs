using System;

namespace TEAM.Common.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public string ServiceUrl { private get; set; }

        public ServiceUnavailableException(string serviceUrl, string message, Exception ex) : base(message, ex)
        {
            ServiceUrl = serviceUrl;
        }
    }
}
