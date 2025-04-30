// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductFamilyCreateRequest
    {
        public required string id { get; set; }
        public required string name { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(name);
        }
    }

    public class ProductFamilyUpdateRequest
    {
        public required string name { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(name);
        }
    }
}
