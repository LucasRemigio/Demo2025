// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.RegularExpressions;
using engimatrix.ModelObjs;
using engimatrix.Utils;

namespace engimatrix.Views
{
    public class OrderProductRequest
    {
        public List<OrderProductItem> products { get; set; }

        public bool IsValid()
        {
            if (products == null || products.Count == 0)
            {
                return false;
            }

            foreach (OrderProductItem item in products)
            {
                if (item.quantity <= 0 || String.IsNullOrEmpty(item.product_catalog_id.ToString()) || item.product_catalog_id <= 0)
                {
                    Log.Debug("Produto inválido: " + item.ToString());
                    return false;
                }
            }

            return true;
        }
    }
}
