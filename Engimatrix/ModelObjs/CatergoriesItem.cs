// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class CategoriesItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string slug { get; set; }

        public CategoriesItem(int id, string title, string slug)
        {
            this.id = id;
            this.title = title;
            this.slug = slug;
        }

        public CategoriesItem ToItem()
        {
            return new CategoriesItem(this.id, this.title, this.slug);
        }
    }
}
