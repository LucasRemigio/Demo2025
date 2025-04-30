// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


public class MFPrimaveraInvoiceTotalItem
{
    public double valor_total { get; set; }
    public double valor_pendente { get; set; }
    public double valor_liquidacao { get; set; }

    public MFPrimaveraInvoiceTotalItem()
    {
        valor_total = 0;
        valor_pendente = 0;
        valor_liquidacao = 0;
    }

    public MFPrimaveraInvoiceTotalItem(double valor_total, double valor_pendente, double valor_liquidacao)
    {
        this.valor_total = valor_total;
        this.valor_pendente = valor_pendente;
        this.valor_liquidacao = valor_liquidacao;
    }
}
