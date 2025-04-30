// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class ClientRatingDTO
{
    public string client_code { get; set; }
    public RatingTypeItem rating_type { get; set; }
    public RatingDiscountItem rating_discount { get; set; }
    public DateTime? updated_at { get; set; }
    public string? updated_by { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }

}


public class ClientRatingDTOBuilder
{
    private readonly ClientRatingDTO clientRatingDTO = new();

    public ClientRatingDTOBuilder SetClientCode(string clientCode)
    {
        clientRatingDTO.client_code = clientCode;
        return this;
    }

    public ClientRatingDTOBuilder SetRatingType(RatingTypeItem ratingType)
    {
        clientRatingDTO.rating_type = ratingType;
        return this;
    }

    public ClientRatingDTOBuilder SetRatingDiscount(RatingDiscountItem ratingDiscount)
    {
        clientRatingDTO.rating_discount = ratingDiscount;
        return this;
    }

    public ClientRatingDTOBuilder SetUpdatedAt(DateTime? updatedAt)
    {
        clientRatingDTO.updated_at = updatedAt;
        return this;
    }

    public ClientRatingDTOBuilder SetUpdatedBy(string? updatedBy)
    {
        clientRatingDTO.updated_by = updatedBy;
        return this;
    }

    public ClientRatingDTOBuilder SetCreatedAt(DateTime createdAt)
    {
        clientRatingDTO.created_at = createdAt;
        return this;
    }

    public ClientRatingDTOBuilder SetCreatedBy(string createdBy)
    {
        clientRatingDTO.created_by = createdBy;
        return this;
    }

    public ClientRatingDTO Build()
    {
        return clientRatingDTO;
    }
}