// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a user already exists.
    /// </summary>
    [Serializable]
    internal class UserExistsException : Exception
    {
#pragma warning disable SA1600

        public UserExistsException()
        {
        }

        public UserExistsException(string message)
            : base(message)
        {
        }

        public UserExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
