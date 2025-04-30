// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    using engimatrix.ModelObjs;

    public class TabelasDBRecord
    {
        public string user_id { get; set; }
        public string user_email { get; set; }

        public string user_name { get; set; }

        public string user_password { get; set; }

        public TabelasDBRecord(string user_id, string user_email, string user_name, string user_password)
        {
            this.user_id = user_id;
            this.user_email = user_email;
            this.user_name = user_name;
            this.user_password = user_password;
        }

        public TabelasItem ToTabelasItem()
        {
            return new TabelasItem(this.user_id, this.user_email, this.user_name, this.user_password);
        }
    }
}
