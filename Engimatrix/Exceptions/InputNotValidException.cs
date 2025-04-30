// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a user is already registred.
    /// </summary>
    [Serializable]
    public class InputNotValidException : Exception
    {
#pragma warning disable SA1600

        public InputNotValidException()
        {
        }

        public InputNotValidException(string message)
            : base(message)
        {
        }

        public InputNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InputNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
