using System;

namespace CryptoTracker.Data.Errors
{
    public class CryptoServiceException : Exception
    {
        public CryptoServiceException(string message)
            :base(message)
        {
            
        }
    }
}
