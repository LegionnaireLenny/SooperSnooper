using System;
using System.Runtime.Serialization;

namespace SooperSnooper.Models.Exceptions
{
    [Serializable]
    public class ProtectedAccountException : Exception
    {
        public ProtectedAccountException()
        {
        }

        public ProtectedAccountException(string message) 
            : base(message)
        {
        }

        public ProtectedAccountException(string message, Exception inner) 
            : base(message, inner)
        {
        }

        protected ProtectedAccountException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}