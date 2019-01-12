using System;

namespace TEAM.Common.Exceptions
{
    public class TFSAuthenticationException : Exception
    {
        public TFSAuthenticationException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
