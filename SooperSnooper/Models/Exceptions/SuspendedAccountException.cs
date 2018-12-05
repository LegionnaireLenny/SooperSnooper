using System;
using System.Runtime.Serialization;

namespace SooperSnooper.Models.Twitter
{
    [Serializable]
    internal class SuspendedAccountException : Exception
    {
        public SuspendedAccountException()
        {
        }

        public SuspendedAccountException(string message) 
            : base(message)
        {
        }

        public SuspendedAccountException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected SuspendedAccountException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}