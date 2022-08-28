using System;
namespace WeightTracking.Application.Exceptions
{
    public class AppBadRequestException : Exception
    {
        public AppBadRequestException(string message) : base(message)
        {
        }
    }
}

