// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ConnectorTypeItem
    {
        public int id { get; set; }
        public string type { get; set; }

        public ConnectorTypeItem(int id, string type)
        {
            this.id = id;
            this.type = type;
        }

        public ConnectorTypeItem ToConnectorType()
        {
            return new ConnectorTypeItem(this.id, this.type);
        }
    }
}
