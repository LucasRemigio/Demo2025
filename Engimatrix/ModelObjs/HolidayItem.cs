// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class HolidayItem
    {
        public int day { get; set; }
        public int month { get; set; }

        public string description { get; set; }

        public HolidayItem(int day, int month, string description)
        {
            this.day = day;
            this.month = month;
            this.description = description;
        }

        public HolidayItem ToItem()
        {
            return new HolidayItem(this.day, this.month, this.description);
        }
    }
}
