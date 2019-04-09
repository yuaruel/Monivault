using System;

namespace Monivault.Exceptions
{
    public class MoneyTransferException : Exception
    {
        public MoneyTransferException(string message):base(message){}
    }
}