// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a user is already registred.
    /// </summary>
    [Serializable]
    internal class UserNotFoundException : Exception
    {
#pragma warning disable SA1600

        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message)
            : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
