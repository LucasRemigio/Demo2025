// // Copyright (c) 2024 Engibots. All rights reserved.


namespace engimatrix.ModelObjs.CTT;
public class CttPostalCodeDto
{
    public int id { get; set; }
    // already includes the district
    public CttMunicipalityDto municipality { get; set; }
    public string llll { get; set; }
    public string localidade { get; set; }
    public string? art_cod { get; set; }
    public string? art_tipo { get; set; }
    public string? pri_prep { get; set; }
    public string? art_titulo { get; set; }
    public string? seg_prep { get; set; }
    public string? art_desig { get; set; }
    public string? art_local { get; set; }
    public string? troco { get; set; }
    public string? porta { get; set; }
    public string? cliente { get; set; }
    public string cp4 { get; set; }
    public string cp3 { get; set; }
    public string cpalf { get; set; }

}

public class CttPostalCodeDtoBuilder
{
    private readonly CttPostalCodeDto _cttPostalCode = new();

    public CttPostalCodeDtoBuilder SetId(int id)
    {
        _cttPostalCode.id = id;
        return this;
    }

    public CttPostalCodeDtoBuilder SetMunicipality(CttMunicipalityDto municipality)
    {
        _cttPostalCode.municipality = municipality;
        return this;
    }

    public CttPostalCodeDtoBuilder SetLlll(string llll)
    {
        _cttPostalCode.llll = llll;
        return this;
    }

    public CttPostalCodeDtoBuilder SetLocalidade(string localidade)
    {
        _cttPostalCode.localidade = localidade;
        return this;
    }

    public CttPostalCodeDtoBuilder SetArtCod(string art_cod)
    {
        _cttPostalCode.art_cod = art_cod;
        return this;
    }

    public CttPostalCodeDtoBuilder SetArtTipo(string art_tipo)
    {
        _cttPostalCode.art_tipo = art_tipo;
        return this;
    }

    public CttPostalCodeDtoBuilder SetPriPrep(string pri_prep)
    {
        _cttPostalCode.pri_prep = pri_prep;
        return this;
    }

    public CttPostalCodeDtoBuilder SetArtTitulo(string art_titulo)
    {
        _cttPostalCode.art_titulo = art_titulo;
        return this;
    }

    public CttPostalCodeDtoBuilder SetSegPrep(string seg_prep)
    {
        _cttPostalCode.seg_prep = seg_prep;
        return this;
    }

    public CttPostalCodeDtoBuilder SetArtDesig(string art_desig)
    {
        _cttPostalCode.art_desig = art_desig;
        return this;
    }

    public CttPostalCodeDtoBuilder SetArtLocal(string art_local)
    {
        _cttPostalCode.art_local = art_local;
        return this;
    }

    public CttPostalCodeDtoBuilder SetTroco(string troco)
    {
        _cttPostalCode.troco = troco;
        return this;
    }

    public CttPostalCodeDtoBuilder SetPorta(string porta)
    {
        _cttPostalCode.porta = porta;
        return this;
    }

    public CttPostalCodeDtoBuilder SetCliente(string cliente)
    {
        _cttPostalCode.cliente = cliente;
        return this;
    }

    public CttPostalCodeDtoBuilder SetCp4(string cp4)
    {
        _cttPostalCode.cp4 = cp4;
        return this;
    }

    public CttPostalCodeDtoBuilder SetCp3(string cp3)
    {
        _cttPostalCode.cp3 = cp3;
        return this;
    }

    public CttPostalCodeDtoBuilder SetCpalf(string cpalf)
    {
        _cttPostalCode.cpalf = cpalf;
        return this;
    }

    public CttPostalCodeDto Build()
    {
        return _cttPostalCode;
    }
}
