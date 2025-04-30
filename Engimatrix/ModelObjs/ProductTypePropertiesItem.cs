// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductTypePropertiesItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string materials { get; set; }
        public string finishings { get; set; }
        public string shapes { get; set; }
        public string surfaces { get; set; }

        public ProductTypePropertiesItem(int id, string name, string materials, string finishings, string shapes, string surfaces)
        {
            this.id = id;
            this.name = name;
            this.materials = materials;
            this.finishings = finishings;
            this.shapes = shapes;
            this.surfaces = surfaces;
        }

        public ProductTypePropertiesItem()
        { }

        public ProductTypePropertiesItem ToItem()
        {
            return new ProductTypePropertiesItem(this.id, this.name, materials, finishings, shapes, surfaces);
        }

        public override string ToString()
        {
            return $"ProductTypeItem:\n" +
                   $"ID: {id}\n" +
                   $"Name: {name}\n" +
                   $"Materials: {materials}\n" +
                   $"Finishings: {finishings}\n" +
                   $"Shapes: {shapes}\n" +
                   $"Surfaces: {surfaces}";
        }

        public bool IsEmpty()
        {
            return id <= 0 &&
                   string.IsNullOrEmpty(name);
        }
    }
}
