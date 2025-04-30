// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ArgumentItem
    {
        public ArgumentTypeItem argument_type { get; set; }
        public ArgumentDirectionItem argument_direction { get; set; }
        public string description { get; set; }

        public ArgumentItem(ArgumentTypeItem argument_type, ArgumentDirectionItem argument_direction, string description)
        {
            this.argument_type = argument_type;
            this.argument_direction = argument_direction;
            this.description = description;
        }

        public ArgumentItem ToArgumentItem()
        {
            return new ArgumentItem(this.argument_type, this.argument_direction, this.description);
        }
    }
}
