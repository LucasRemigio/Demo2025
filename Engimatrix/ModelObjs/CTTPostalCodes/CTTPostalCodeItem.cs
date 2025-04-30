// // Copyright (c) 2024 Engibots. All rights reserved.


namespace engimatrix.ModelObjs.CTT;
public class CttPostalCodeItem
{
    public int id { get; set; }
    public string dd { get; set; }
    public string cc { get; set; }
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

public class CttPostalCodeItemBuilder
{
    private readonly CttPostalCodeItem _cttPostalCode = new();

    public CttPostalCodeItemBuilder SetId(int id)
    {
        _cttPostalCode.id = id;
        return this;
    }

    public CttPostalCodeItemBuilder SetDd(string dd)
    {
        _cttPostalCode.dd = dd;
        return this;
    }

    public CttPostalCodeItemBuilder SetCc(string cc)
    {
        _cttPostalCode.cc = cc;
        return this;
    }

    public CttPostalCodeItemBuilder SetLlll(string llll)
    {
        _cttPostalCode.llll = llll;
        return this;
    }

    public CttPostalCodeItemBuilder SetLocalidade(string localidade)
    {
        _cttPostalCode.localidade = localidade;
        return this;
    }

    public CttPostalCodeItemBuilder SetArtCod(string? artCod)
    {
        _cttPostalCode.art_cod = artCod;
        return this;
    }

    public CttPostalCodeItemBuilder SetArtTipo(string? artTipo)
    {
        _cttPostalCode.art_tipo = artTipo;
        return this;
    }

    public CttPostalCodeItemBuilder SetPriPrep(string? priPrep)
    {
        _cttPostalCode.pri_prep = priPrep;
        return this;
    }

    public CttPostalCodeItemBuilder SetArtTitulo(string? artTitulo)
    {
        _cttPostalCode.art_titulo = artTitulo;
        return this;
    }

    public CttPostalCodeItemBuilder SetSegPrep(string? segPrep)
    {
        _cttPostalCode.seg_prep = segPrep;
        return this;
    }

    public CttPostalCodeItemBuilder SetArtDesig(string? artDesig)
    {
        _cttPostalCode.art_desig = artDesig;
        return this;
    }

    public CttPostalCodeItemBuilder SetArtLocal(string? artLocal)
    {
        _cttPostalCode.art_local = artLocal;
        return this;
    }

    public CttPostalCodeItemBuilder SetTroco(string? troco)
    {
        _cttPostalCode.troco = troco;
        return this;
    }

    public CttPostalCodeItemBuilder SetPorta(string? porta)
    {
        _cttPostalCode.porta = porta;
        return this;
    }

    public CttPostalCodeItemBuilder SetCliente(string? cliente)
    {
        _cttPostalCode.cliente = cliente;
        return this;
    }

    public CttPostalCodeItemBuilder SetCp4(string cp4)
    {
        _cttPostalCode.cp4 = cp4;
        return this;
    }

    public CttPostalCodeItemBuilder SetCp3(string cp3)
    {
        _cttPostalCode.cp3 = cp3;
        return this;
    }

    public CttPostalCodeItemBuilder SetCpalf(string cpalf)
    {
        _cttPostalCode.cpalf = cpalf;
        return this;
    }

    public CttPostalCodeItem Build()
    {
        return _cttPostalCode;
    }
}
