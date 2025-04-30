// // Copyright (c) 2024 Engibots. All rights reserved.
namespace engimatrix.ModelObjs
{
    public class TransportItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
    }

    public class TransportItemBuilder
    {
        private readonly TransportItem _transportItem = new();

        public TransportItemBuilder SetId(int id)
        {
            _transportItem.id = id;
            return this;
        }

        public TransportItemBuilder SetName(string name)
        {
            _transportItem.name = name;
            return this;
        }

        public TransportItemBuilder SetSlug(string slug)
        {
            _transportItem.slug = slug;
            return this;
        }

        public TransportItemBuilder SetDescription(string description)
        {
            _transportItem.description = description;
            return this;
        }

        public TransportItem Build()
        {
            return _transportItem;
        }
    }
}
