// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class HolidaysResponse
    {
        public class GetAllHolidays
        {
            public List<HolidayItem> holidays { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAllHolidays(List<HolidayItem> holidays, int result_code, string language)
            {
                this.holidays = holidays;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAllHolidays(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
