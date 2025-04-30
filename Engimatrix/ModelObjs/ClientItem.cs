// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ClientItem
    {
        public int id { get; set; }
        public string token { get; set; }
        public string code { get; set; }
        public int segment_id { get; set; }
        public DateTime updated_at { get; set; }
        public string updated_by { get; set; }
        public DateTime created_at { get; set; }
        public string created_by { get; set; }
    }

    public class ClientItemBuilder
    {
        private readonly ClientItem clientItem = new();

        public ClientItemBuilder SetId(int id)
        {
            clientItem.id = id;
            return this;
        }

        public ClientItemBuilder SetCode(string code)
        {
            clientItem.code = code;
            return this;
        }

        public ClientItemBuilder SetToken(string token)
        {
            clientItem.token = token;
            return this;
        }

        public ClientItemBuilder SetSegmentId(int segmentId)
        {
            clientItem.segment_id = segmentId;
            return this;
        }

        public ClientItemBuilder SetUpdatedAt(DateTime updatedAt)
        {
            clientItem.updated_at = updatedAt;
            return this;
        }

        public ClientItemBuilder SetUpdatedBy(string updatedBy)
        {
            clientItem.updated_by = updatedBy;
            return this;
        }

        public ClientItemBuilder SetCreatedAt(DateTime createdAt)
        {
            clientItem.created_at = createdAt;
            return this;
        }

        public ClientItemBuilder SetCreatedBy(string createdBy)
        {
            clientItem.created_by = createdBy;
            return this;
        }

        public ClientItem Build()
        {
            return clientItem;
        }
    }
}
