// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductFamilyItem
    {
        public string id { get; set; } 
        public string name { get; set; } 

        public ProductFamilyItem(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public ProductFamilyItem()
        { }

        public ProductFamilyItem ToItem()
        {
            return new ProductFamilyItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductFamilyItem:\n" +
                   $"ID: {id}\n" +
                   $"Name: {name}\n";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(id) &&
                   string.IsNullOrEmpty(name);
        }
    }
}
