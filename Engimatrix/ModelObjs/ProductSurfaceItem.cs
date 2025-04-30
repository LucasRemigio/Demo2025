// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductSurfaceItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductSurfaceItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductSurfaceItem()
        { }

        public ProductSurfaceItem ToItem()
        {
            return new ProductSurfaceItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductSurfaceItem:\n" +
                   $"ID: {id}\n" +
                   $"Name: {name}\n";
        }

        public bool IsEmpty()
        {
            return id <= 0 &&
                   string.IsNullOrEmpty(name);
        }
    }
}
