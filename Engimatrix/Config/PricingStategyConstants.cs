// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using Engimatrix.ModelObjs;
using static engimatrix.Config.RatingConstants;

namespace engimatrix.Config
{
    public enum PricingStrategyId
    {
        LAST_PRICE = 1,
        AVERAGE_PRICE = 2,
    }

}

