// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during the submission of receipt and an error is found.
    /// </summary>
    [Serializable]
    internal class UnableToProcessFileException : Exception
    {
#pragma warning disable SA1600

        public UnableToProcessFileException()
        {
        }

        public UnableToProcessFileException(string message)
            : base(message)
        {
        }

        public UnableToProcessFileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnableToProcessFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
