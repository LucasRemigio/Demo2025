// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductShapeItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductShapeItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductShapeItem()
        { }

        public ProductShapeItem ToItem()
        {
            return new ProductShapeItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductShapeItem:\n" +
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
