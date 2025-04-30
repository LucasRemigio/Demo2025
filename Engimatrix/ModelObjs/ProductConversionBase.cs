// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductConversionBase
    {
        public int id { get; set; }
        public string product_code { get; set; }
        public decimal rate { get; set; }

        public override string ToString()
        {
            return $"id: {id}\n, product_code: {product_code}\n, rate: {rate}\n";
        }
    }

}