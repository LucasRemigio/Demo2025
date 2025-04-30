// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class SegmentItem
    {
        public int id { get; set; }
        public string name { get; set; }

        public SegmentItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public SegmentItem(string name)
        {
            this.name = name;
        }

        public SegmentItem()
        { }

        public SegmentItem ToItem()
        {
            return new SegmentItem(this.id, this.name);
        }

        public override string ToString()
        {
            return $"ProductFamily:\n" +
                   $"ID: {id}\n" +
                   $"Name: {name}\n";
        }

        public bool IsEmpty()
        {
            return id == 0 &&
                   string.IsNullOrEmpty(name);
        }
    }
}
