// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using static Engimatrix.ModelObjs.SmartSheet;

namespace Engimatrix.ModelObjs;
public class CategoryItem
{
    public int id { get; set; }
    public string title { get; set; }
    public string slug { get; set; }

    public CategoryItem(int id, string title, string slug)
    {
        this.id = id;
        this.title = title;
        this.slug = slug;
    }

}

