// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Drawing;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
using Engimatrix.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Graph.Drives.Item.Items.Item.GetActivitiesByIntervalWithStartDateTimeWithEndDateTimeWithInterval;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace engimatrix.Models
{
    public static class ProductModel
    {
        public static ProductItem SaveProduct(ProductItem product, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    { "email_token", product.email_token },
                    { "name", product.name },
                    { "size", product.size },
                    { "quantity", product.quantity.ToString()  },
                    { "quantity_unit", product.quantity_unit },
                    { "confidence", product.confidence.ToString() },
                    { "product_catalog_id", product.product_catalog_id?.ToString() },
                    { "match_confidence", product.match_confidence?.ToString() },
                    { "calculated_price", product.calculated_price?.ToString() }
                };

            string query = "INSERT INTO email_product (`email_token`, `name`, `size`, `quantity`, `quantity_unit`, `confidence`, `product_catalog_id`, `match_confidence`, `calculated_price`) " +
                           "VALUES (@email_token, @name, @size, @quantity, @quantity_unit, @confidence, @product_catalog_id, @match_confidence, @calculated_price)";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "InsertProductRecord");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong inserting the product to the database");
            }

            return product;
        }

        public static ProductItem SaveProduct(ProductItem product)
        {
            return SaveProduct(product, "");
        }

        public static List<ProductItem> GetEmailProducts(string emailToken, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("emailToken", emailToken);

            string query = "SELECT p.id, p.email_token, p.name, p.quantity, p.quantity_unit, p.size, p.confidence, " +
                "p.product_catalog_id, p.match_confidence, p.calculated_price " +
                "FROM email_product p " +
                "WHERE p.email_token = @emailToken";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailProducts");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong getting the product from the database");
            }

            List<ProductItem> products = new List<ProductItem>();

            if (response.out_data.Count == 0)
            {
                // No products found for the email token
                return products;
            }

            foreach (Dictionary<string, string> item in response.out_data)
            {
                int id = Int32.Parse(item["0"]);
                string email_token = item["1"];
                string name = item["2"];
                int quantity = Int32.Parse(item["3"].Split(".")[0]);
                string quantity_unit = item["4"];
                string size = item["5"];
                int confidence = Int32.Parse(item["6"]);
                int product_catalog_id = Int32.Parse(item["7"]);
                int match_confidence = Int32.Parse(item["8"]);
                decimal calculated_price = Decimal.Parse(item["9"], CultureInfo.InvariantCulture);

                ProductItem product = new ProductItem(id, email_token, name, size, quantity, quantity_unit, confidence, product_catalog_id, match_confidence, calculated_price);
                products.Add(product);
            }

            return products;
        }

        public static List<ProductDTO> GetProductsDTOByEmailId(int emailId, string execute_user)
        {
            Dictionary<string, string> dic = [];
            dic.Add("emailId", emailId.ToString());

            //                      0          1          2         3             4              5         6
            string query = "SELECT p.id, p.email_token, p.name, p.quantity, p.quantity_unit, p.size, p.confidence, " +
                //    7                     8                    9                   10            11           12
                "p.match_confidence, p.calculated_price, p.product_catalog_id, pc.description, pc.price_pvp, pc.product_code " +

                "FROM email_product p " +
                "JOIN filtered_email f ON p.email_token = f.token " +
                "LEFT JOIN mf_product_catalog pc ON pc.id = p.product_catalog_id " +
                "WHERE f.email = @emailId";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetProductsByEmailId");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong getting the product from the database");
            }

            if (response.out_data.Count == 0)
            {
                // No products found for the email token
                return [];
            }

            List<ProductDTO> products = [];
            foreach (Dictionary<string, string> item in response.out_data)
            {
                int id = Int32.Parse(item["0"]);
                string email_token = item["1"];
                string name = item["2"];
                decimal quantity = Decimal.Parse(item["3"]);
                string quantity_unit = item["4"];
                string size = item["5"];
                int confidence = Int32.Parse(item["6"]);
                int match_confidence = Int32.Parse(item["7"]);
                decimal calculated_price = Decimal.Parse(item["8"]);

                int productCatalogId = Int32.Parse(String.IsNullOrEmpty(item["9"]) ? "0" : item["9"]);

                // Only append product catalog if there is a product catalog id
                if (productCatalogId == 0)
                {
                    ProductDTO productWithoutCatalog = new(id, email_token, name, size, quantity, quantity_unit, confidence, null, match_confidence, calculated_price);
                    products.Add(productWithoutCatalog);
                    continue;
                }

                decimal pricePvp = Decimal.Parse(item["11"]);

                ProductCatalogDTO productCatalogDTO = new(productCatalogId, item["10"], item["12"], pricePvp);

                ProductDTO productDTO = new(id, email_token, name, size, quantity, quantity_unit, confidence, productCatalogDTO, match_confidence, calculated_price);
                products.Add(productDTO);
            }

            return products;
        }

        public static void UpdateProduct(ProductItem product, string execute_user)
        {
            if (product.id <= 0)
            {
                throw new ArgumentException("Invalid product ID for update.");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", product.id.ToString());
            dic.Add("email_token", product.email_token);
            dic.Add("name", product.name);
            dic.Add("size", product.size);
            dic.Add("quantity", product.quantity.ToString());
            dic.Add("quantity_unit", product.quantity_unit);
            dic.Add("confidence", product.confidence.ToString());

            string query = "UPDATE email_product " +
                           "SET email_token = @email_token, name = @name, size = @size, quantity = @quantity, quantity_unit = @quantity_unit, confidence = @confidence " +
                           "WHERE id = @id";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateProduct");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong updating the product in the database.");
            }
        }

        public static void DeleteProduct(ProductItem product, string execute_user)
        {
            if (product.id <= 0)
            {
                throw new ArgumentException("Invalid product ID for deletion.");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", product.id.ToString());

            string query = "DELETE FROM email_product WHERE id = @id";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteProduct");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong deleting the product from the database.");
            }
        }

        public static void DeleteProductList(List<ProductItem> productList, string execute_user)
        {
            List<int> ids = new List<int>();
            foreach (ProductItem product in productList)
            {
                if (product.id <= 0)
                {
                    throw new ArgumentException("Invalid product ID for deletion.");
                }
                ids.Add(product.id);
            }

            // Convert the list of IDs to a comma-separated string
            string idsCommaSeparated = string.Join(", ", ids);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            string query = $"DELETE FROM email_product WHERE id in ({idsCommaSeparated})";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteProduct");

            if (!response.operationResult)
            {
                throw new Exception("Something went wrong deleting the product from the database.");
            }
        }

        public static void UpdateEmailProducts(string emailToken, List<ProductItem> products, string execute_user)
        {
            List<ProductItem> originalProducts = GetEmailProducts(emailToken, execute_user);

            /*
             * Use cases to validate while comparing item for item:
             * If id is the same: check if values have changed
             *      If values have changed, update that item
             * If original had x id but now it doesnt: the item was eliminated
             * If products list has an id that didnt have before: the item is new
             */

            foreach (ProductItem product in products)
            {
                // Check if the product already exists in the original list (by ID)
                ProductItem originalProduct = originalProducts.FirstOrDefault(p => p.id == product.id);

                // Current product was found on the original products list
                if (originalProduct != null)
                {
                    if (HasProductChanged(originalProduct, product))
                    {
                        // Values have changed, update product
                        UpdateProduct(product, execute_user);
                    }

                    // Remove the product from the original list since it is handled
                    originalProducts.Remove(originalProduct);
                }
                else
                {
                    // Product is new (does not exist in the original list)
                    SaveProduct(product, execute_user);
                }
            }

            // Remaining items in originalProducts are the ones that were eliminated
            List<ProductItem> productsToRemove = originalProducts;
            if (productsToRemove.Count > 0)
            {
                DeleteProductList(productsToRemove, execute_user);
            }

            // Update email status to Filtragem_Realizada
            FilteringModel.ChangeEmailStatus(emailToken, StatusConstants.StatusCode.TRIAGEM_REALIZADA.ToString(), execute_user);
        }

        private static bool HasProductChanged(ProductItem original, ProductItem updated)
        {
            return original.name != updated.name ||
                   original.size != updated.size ||
                   original.quantity != updated.quantity ||
                   original.quantity_unit != updated.quantity_unit ||
                   original.confidence != updated.confidence;
        }

        public static void SaveProductFromOpenAi(ProdutoComparado product, decimal price, string token)
        {
            // validate all the fields. all the fields can come empty or badly formatted (ex. confidence is number but can come as "Alta" instead of 90)
            string productName = product.NomeProdutoSolicitado ?? "";
            string productSize = product.MedidasProdutoSolicitado ?? "";
            decimal productQuantity = Decimal.Parse(product.Quantidade ?? "0");
            string productQuantityUnit = product.UnidadeQuantidade ?? "";

            int productConfidence = Int32.Parse(product.Confianca ?? "0");
            int? productCatalogId = int.TryParse(product.IdProdutoCatalogo, out int catalogId) ? catalogId : (int?)null;
            int? matchConfidence = int.TryParse(product.Confianca, out int matchConf) ? matchConf : (int?)null;

            ProductItem productItem = new(token, productName, productSize, productQuantity, productQuantityUnit, productConfidence, productCatalogId, matchConfidence, price);
            SaveProduct(productItem);
        }
    }
}
