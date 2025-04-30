// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a user is already registred.
    /// </summary>
    [Serializable]
    public class ItemAlreadyExistsException : Exception
    {
#pragma warning disable SA1600

        public ItemAlreadyExistsException()
        {
        }

        public ItemAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ItemAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ItemAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
