// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using Engimatrix.ModelObjs;
using static engimatrix.Config.RatingConstants;

namespace engimatrix.Config;


public enum HereRouteTransportMode
{
    /*
    transportation mode, which can be bicycle, bus, car,pedestrian,scooter, taxi, or truck
    */
    Bicycle,
    Bus,
    Car,
    Pedestrian,
    Scooter,
    Taxi,
    Truck
}
