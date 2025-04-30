// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;
public class StatusItem
{
    public int id { get; set; }
    public string description { get; set; }

    public StatusItem(int id, string description)
    {
        this.id = id;
        this.description = description;
    }
}
