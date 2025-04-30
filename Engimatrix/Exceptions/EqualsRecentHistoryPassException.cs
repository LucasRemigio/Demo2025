// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class EqualsRecentHistoryPassException : Exception
    {
#pragma warning disable SA1600

        public EqualsRecentHistoryPassException()
        {
        }

        public EqualsRecentHistoryPassException(string message)
            : base(message)
        {
        }

        public EqualsRecentHistoryPassException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EqualsRecentHistoryPassException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
