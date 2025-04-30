// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.RegularExpressions;
using engimatrix.ModelObjs;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Views
{
    public class OrderUpdateRequest
    {
        public OrderItem order { get; set; }

        public bool isValid()
        {
            /*
             *      ^\d{4}: Starts with exactly four digits.
             *      -: Followed by a hyphen.
             *      \d{3}$: Ends with exactly three digits.
             */
            string postalCodePattern = @"^\d{4}-\d{3}$";
            /*
            *      ^[a-zA-Z0-9\s,'\.\-áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]{1,250}$:
            *      - Allows letters (including accented characters), digits, spaces, commas, periods, apostrophes, and hyphens.
            *      - {1,250}: Limits the address length between 1 and 250 characters.
            */
            string addressPattern = @"^[a-zA-Z0-9\s,'\.\-áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]{1,250}$";

            if (!Regex.IsMatch(this.order.postal_code, postalCodePattern))
            {
                return false;
            }

            if (!Regex.IsMatch(this.order.address, addressPattern))
            {
                return false;
            }

            if (this.order.token.IsNullOrEmpty() || this.order.token.Length > 50)
            {
                return false;
            }

            if (this.order.email_token.IsNullOrEmpty() || this.order.email_token.Length > 50)
            {
                return false;
            }

            return true;
        }
    }
}
