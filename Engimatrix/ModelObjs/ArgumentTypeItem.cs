// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ArgumentTypeItem
    {
        public string description { get; set; }

        public ArgumentTypeItem(string description)
        {
            this.description = description;
        }

        public ArgumentTypeItem ToArgumentTypeItem()
        {
            return new ArgumentTypeItem(this.description);
        }
    }
}
