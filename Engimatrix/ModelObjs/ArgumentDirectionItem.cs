// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ArgumentDirectionItem
    {
        public string description { get; set; }

        public ArgumentDirectionItem(string description)
        {
            this.description = description;
        }

        public ArgumentDirectionItem ToArgumentDirectionItem()
        {
            return new ArgumentDirectionItem(this.description);
        }
    }
}
