// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductTypeItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductTypeItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductTypeItem()
        { }

        public ProductTypeItem ToItem()
        {
            return new ProductTypeItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductTypeItem:\n" +
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
