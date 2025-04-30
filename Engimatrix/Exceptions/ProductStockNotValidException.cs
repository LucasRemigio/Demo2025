// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised when the stock of a product is not valid.
    /// </summary>
    [Serializable]
    public class ProductStockNotValidException : Exception
    {
#pragma warning disable SA1600

        public ProductStockNotValidException()
        {
        }

        public ProductStockNotValidException(string message)
            : base(message)
        {
        }

        public ProductStockNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ProductStockNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
