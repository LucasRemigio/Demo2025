// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Orquestration
{
    public class AssetsItem
    {
        public int id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string text { get; set; }

        public AssetsItem(int id, string description, string type, string user, string password, string text)
        {
            this.id = id;
            this.description = description;
            this.type = type;
            this.user = user;
            this.password = password;
            this.text = text;
        }

        public AssetsItem ToItem()
        {
            return new AssetsItem(this.id, this.description, this.type, this.user, this.password, this.text);
        }
    }
}
