using System;
namespace WeightTracking.Application.Exceptions
{
    public class AppNotFoundException : Exception
    {
        public AppNotFoundException(string message) : base(message)
        {
        }
    }
}

