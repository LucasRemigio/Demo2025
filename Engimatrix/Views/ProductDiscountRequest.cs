// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;

public class ProductDiscountCreateRequest
{
    public required string product_family_id { get; set; }
    public required int segment_id { get; set; }
    public required decimal mb_min { get; set; }
    public required decimal desc_max { get; set; }

    public bool IsValid()
    {
        return !String.IsNullOrEmpty(product_family_id) && segment_id > 0 && mb_min > 0 && desc_max > 0;
    }
}

public class ProductDiscountUpdateRequest
{
    public required decimal mb_min { get; set; }
    public required decimal desc_max { get; set; }

    public bool IsValid()
    {
        return mb_min > 0 && desc_max > 0;
    }
}
