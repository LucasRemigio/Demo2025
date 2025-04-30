// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;
public class DestinationDetailsItem
{
    public string destination { get; set; }
    public int distance { get; set; }
    public int duration { get; set; }

    public DestinationDetailsItem(string destination, int distance, int duration)
    {
        this.destination = destination;
        this.distance = distance;
        this.duration = duration;
    }

    public DestinationDetailsItem()
    {
    }
}
