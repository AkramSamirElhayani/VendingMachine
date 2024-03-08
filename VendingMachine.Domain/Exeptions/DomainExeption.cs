using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Exeptions
{
    public abstract class DomainExeption : Exception
    {
        protected DomainExeption()
        {
        }

        protected DomainExeption(string? message) : base(message)
        {
        }

        protected DomainExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected DomainExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
