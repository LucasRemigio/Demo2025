// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    using engimatrix.ModelObjs;

    public class GetAllLogsDBRRecord
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public string NameShort { get; set; }
        public string State { get; set; }

        public GetAllLogsDBRRecord(string email, string id, string nameShort, string state)
        {
            this.Email = email;
            this.Id = id;
            this.NameShort = nameShort;
            this.State = state;
        }

        public GetAllLogsItem ToGetAllLogsItem()
        {
            return new GetAllLogsItem(this.Email, this.NameShort, this.State, this.Id);
        }
    }
}
