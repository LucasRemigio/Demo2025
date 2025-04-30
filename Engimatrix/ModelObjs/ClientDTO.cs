// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Primavera;

namespace engimatrix.ModelObjs;
public class ClientDTO
{
    public int id { get; set; }

    public string token { get; set; }
    public string code { get; set; }
    public SegmentItem segment { get; set; }
    public MFPrimaveraClientItem? primavera_client { get; set; }
    public List<ClientRatingDTO>? ratings { get; set; }
    public DateTime? updated_at { get; set; }
    public string? updated_by { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }
    public decimal weighted_rating { get; set; }
}

public class ClientDTOBuilder
{
    private readonly ClientDTO _clientDTO = new();

    public ClientDTOBuilder SetId(int id)
    {
        _clientDTO.id = id;
        return this;
    }

    public ClientDTOBuilder SetCode(string code)
    {
        _clientDTO.code = code;
        return this;
    }

    public ClientDTOBuilder SetSegment(SegmentItem segment)
    {
        _clientDTO.segment = segment;
        return this;
    }

    public ClientDTOBuilder SetPrimaveraClient(MFPrimaveraClientItem primavera_client)
    {
        _clientDTO.primavera_client = primavera_client;
        return this;
    }

    public ClientDTOBuilder SetRatings(List<ClientRatingDTO> ratings)
    {
        _clientDTO.ratings = ratings;
        return this;
    }

    public ClientDTOBuilder SetUpdatedAt(DateTime? updated_at)
    {
        _clientDTO.updated_at = updated_at;
        return this;
    }

    public ClientDTOBuilder SetUpdatedBy(string? updated_by)
    {
        _clientDTO.updated_by = updated_by;
        return this;
    }

    public ClientDTOBuilder SetCreatedAt(DateTime created_at)
    {
        _clientDTO.created_at = created_at;
        return this;
    }

    public ClientDTOBuilder SetCreatedBy(string created_by)
    {
        _clientDTO.created_by = created_by;
        return this;
    }

    public ClientDTOBuilder SetWeightedRating(decimal weighted_rating)
    {
        _clientDTO.weighted_rating = weighted_rating;
        return this;
    }

    public ClientDTOBuilder SetToken(string token)
    {
        _clientDTO.token = token;
        return this;
    }

    public ClientDTO Build()
    {
        return _clientDTO;
    }

}

public class ClientNoAuthDTO
{
    public string token { get; set; }
    public string code { get; set; }
    public SegmentItem segment { get; set; }
    public MFPrimaveraClientItemNoAuth? primavera_client { get; set; }
}


public class ClientNoAuthDTOBuilder
{
    private readonly ClientNoAuthDTO _clientNoAuthDTO = new();

    public ClientNoAuthDTOBuilder SetToken(string token)
    {
        _clientNoAuthDTO.token = token;
        return this;
    }

    public ClientNoAuthDTOBuilder SetCode(string code)
    {
        _clientNoAuthDTO.code = code;
        return this;
    }

    public ClientNoAuthDTOBuilder SetSegment(SegmentItem segment)
    {
        _clientNoAuthDTO.segment = segment;
        return this;
    }

    public ClientNoAuthDTOBuilder SetPrimaveraClient(MFPrimaveraClientItemNoAuth primavera_client)
    {
        _clientNoAuthDTO.primavera_client = primavera_client;
        return this;
    }

    public ClientNoAuthDTO Build()
    {
        return _clientNoAuthDTO;
    }
}
