using System.Collections.Generic;
using Engimatrix.ModelObjs;

namespace engimatrix.Config;

public static class ProductUnitConstants
{
    public enum Unit
    {
        // Make first start at 1 so it matches the database id's
        KG = 1,       // Kilogram
        MT,       // Meter
        UN,       // Unit
        M2,       // Meter Squared
        RL,        // Roll
        ML,       // Milliliter
    }
}
