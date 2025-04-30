// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductFinishingItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductFinishingItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductFinishingItem()
        { }

        public ProductFinishingItem ToItem()
        {
            return new ProductFinishingItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductFinishingItem:\n" +
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
