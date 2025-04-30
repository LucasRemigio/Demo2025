// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Reflection;
using Engimatrix.ModelObjs;

namespace engimatrix.Config
{
    // This enum aims to represent the status of an order rating change request, that could be one of the following:
    // To access them, do .ToString().ToLower()
    public enum OrderRatingStatus
    {
        Pending,
        Accepted,
        Rejected
    }

}
