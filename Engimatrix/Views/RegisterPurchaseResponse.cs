// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Views
{
    public class RegisterPurchaseResponse
    {
        private string id;
        private string number;
        private string guid;
        private string deliveryDate;

        public RegisterPurchaseResponse(string id, string number, string guid, string deliveryDate)
        {
            this.Id = id;
            this.Number = number;
            this.Guid = guid;
            this.DeliveryDate = deliveryDate;
        }

        public string Id { get => id; set => id = value; }
        public string Number { get => number; set => number = value; }
        public string Guid { get => guid; set => guid = value; }
        public string DeliveryDate { get => deliveryDate; set => deliveryDate = value; }

        public override string ToString()
        {
            return "Id: " + Id + ", Number: " + Number + ", Guid: " + Guid + ", Delivery Date: " + Convert.ToDateTime(DeliveryDate).ToString("dd/MM/yyyy");
        }
    }
}
