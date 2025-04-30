// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Utils.QRCode
{
    public class QrCodeInvoiceConverter
    {
        public string nif_emitente { get; set; }
        public string nif_adquirente { get; set; }
        public string pais_adquirente { get; set; }
        public string tipo_documento { get; set; }
        public string estado_documento { get; set; }
        public string data_documento { get; set; }
        public string identificacao_unica_documento { get; set; }
        public string atcud { get; set; }
        public string espaco_fiscal { get; set; }
        public string base_tributavel_iva_taxa_normal { get; set; }
        public string total_iva_taxa_normal { get; set; }
        public string total_impostos { get; set; }
        public string total_documento_impostos { get; set; }
        public string quatro_carateres_hash { get; set; }
        public string nr_certificado { get; set; }
        public string outras_informações { get; set; }
    }
}
