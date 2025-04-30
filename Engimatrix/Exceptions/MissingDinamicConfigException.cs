// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Runtime.Serialization;
using engimatrix.Notifications;

namespace engimatrix.Exceptions
{
    [Serializable]
    internal class MissingDinamicConfigException : Exception
    {
        public MissingDinamicConfigException()
        {
        }

        public MissingDinamicConfigException(string message) : base(message)
        {
            PlatformAlerts.CreateCriticalPlatformAlert(message);
        }

        public MissingDinamicConfigException(string message, Exception innerException) : base(message, innerException)
        {
            PlatformAlerts.CreateCriticalPlatformAlert(message);
        }

        protected MissingDinamicConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
