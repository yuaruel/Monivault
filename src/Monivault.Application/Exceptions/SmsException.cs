using System;

namespace Monivault.Exceptions
{
    public class SmsException : Exception
    {
        public SmsException(string message):base(message){}
    }
}