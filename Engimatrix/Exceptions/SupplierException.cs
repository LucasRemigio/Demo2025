// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a employee already exists.
    /// </summary>
    [Serializable]
    internal class SupplierNotFoundException : Exception
    {
#pragma warning disable SA1600

        public SupplierNotFoundException(string message) : base(message)
        {
        }

#pragma warning restore SA1600
    }
}
