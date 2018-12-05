using System;
using System.Runtime.Serialization;

namespace SooperSnooper.Models.Twitter
{
    [Serializable]
    public class NoTweetsFoundException : Exception
    {
        public NoTweetsFoundException()
        {
        }

        public NoTweetsFoundException(string message) 
            : base(message)
        {
        }

        public NoTweetsFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected NoTweetsFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}