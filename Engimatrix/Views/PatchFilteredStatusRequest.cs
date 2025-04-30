// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel.DataAnnotations;
using engimatrix.Config;
using engimatrix.Utils;
using Smartsheet.Api.Models;

namespace engimatrix.Views
{
    public class PatchFilteredStatusRequest
    {
        public string email_token { get; set; }

        public int status_id { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputString(email_token) || status_id <= 0 || status_id >= StatusConstants.ULTIMO_ID_VALIDO + 1)
            {
                return false;
            }

            return true;
        }
    }
}
