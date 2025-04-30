// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductMaterialItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductMaterialItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductMaterialItem()
        { }

        public ProductMaterialItem ToItem()
        {
            return new ProductMaterialItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductMaterialItem:\n" +
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
