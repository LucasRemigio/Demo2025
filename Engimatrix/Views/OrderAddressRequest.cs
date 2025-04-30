// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.RegularExpressions;
using engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class OrderAddressRequest
    {
        public bool is_delivery { get; set; }
        public string? postal_code_cp4 { get; set; }
        public string? postal_code_cp3 { get; set; }
        public string? address { get; set; }
        public int? transport_id { get; set; }

        public bool IsValid()
        {
            if (is_delivery == false)
            {
                return true;
            }

            // The cp4 must be 4 digits
            if (string.IsNullOrEmpty(this.postal_code_cp4) || this.postal_code_cp4.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(this.postal_code_cp4, out int _))
            {
                return false;
            }

            // The cp3 must be 3 digits
            if (string.IsNullOrEmpty(this.postal_code_cp3) || this.postal_code_cp3.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(this.postal_code_cp3, out int _))
            {
                return false;
            }

            /*
            *      ^[a-zA-Z0-9\s,'\.\-áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]{1,250}$:
            *      - Allows letters (including accented characters), digits, spaces, commas, periods, apostrophes, and hyphens.
            *      - {1,250}: Limits the address length between 1 and 250 characters.
            */
            string addressPattern = @"^[a-zA-Z0-9\s,'\.\-áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]{1,250}$";

            if (string.IsNullOrEmpty(this.address) || !Regex.IsMatch(this.address, addressPattern))
            {
                return false;
            }

            if (transport_id <= 0 || transport_id > 3)
            {
                return false;
            }

            return true;
        }
    }

}
