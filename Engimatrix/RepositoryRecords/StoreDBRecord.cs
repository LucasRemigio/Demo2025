// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    public class StoreDBRecord
    {
        public string IdStoreCode { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }

        public StoreDBRecord(string IdStoreCode, string StoreCode, string StoreName)
        {
            this.IdStoreCode = IdStoreCode;
            this.StoreCode = StoreCode;
            this.StoreName = StoreName;
        }
    }
}
