// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class AveragePaymentTimeItem
    {
        public float? average_payment_time_days { get; set; }
        public float? average_deadline_time_days { get; set; }

    }

    public class AveragePaymentTimeItemBuilder
    {
        private readonly AveragePaymentTimeItem _averagePaymentTimeItem = new();

        public AveragePaymentTimeItemBuilder SetAveragePaymentTimeDays(float? averagePaymentTimeDays)
        {
            _averagePaymentTimeItem.average_payment_time_days = averagePaymentTimeDays;
            return this;
        }

        public AveragePaymentTimeItemBuilder SetAverageDeadlineTimeDays(float? averageDeadlineTimeDays)
        {
            _averagePaymentTimeItem.average_deadline_time_days = averageDeadlineTimeDays;
            return this;
        }

        public AveragePaymentTimeItem Build()
        {
            return _averagePaymentTimeItem;
        }
    }
}
