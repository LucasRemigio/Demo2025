// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Runtime.Serialization;

namespace engimatrix.Exceptions
{
    [Serializable]
    internal class InvalidConfigValueException : Exception
    {
        public InvalidConfigValueException()
        {
        }

        public InvalidConfigValueException(string message) : base(message)
        {
        }

        public InvalidConfigValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidConfigValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
