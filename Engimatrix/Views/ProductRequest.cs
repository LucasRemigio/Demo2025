// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class UpdateProductRequest
    {
        public List<ProductItem> products { get; set; }

        public bool Validate()
        {
            foreach (ProductItem product in products)
            {
                if (string.IsNullOrWhiteSpace(product.name) ||
                       string.IsNullOrWhiteSpace(product.size) ||
                       product.quantity <= 0 ||
                       string.IsNullOrWhiteSpace(product.quantity.ToString()) ||
                       string.IsNullOrWhiteSpace(product.quantity_unit))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
