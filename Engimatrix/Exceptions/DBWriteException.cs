// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Runtime.Serialization;
using engimatrix.Notifications;

namespace engimatrix.Exceptions
{
    [Serializable]
    internal class DBWriteException : Exception
    {
        public DBWriteException()
        {
        }

        public DBWriteException(string message) : base(message)
        {
            PlatformAlerts.CreateCriticalPlatformAlert(message);
        }

        public DBWriteException(string message, Exception innerException) : base(message, innerException)
        {
            PlatformAlerts.CreateCriticalPlatformAlert(message);
        }

        protected DBWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
