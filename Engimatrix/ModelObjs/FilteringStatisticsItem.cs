// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs
{
    public class FilteringStatisticsItem
    {
        public int total { get; set; }
        public int automatic { get; set; }
        public int manual { get; set; }
        public int toValidate { get; set; }
        public int error { get; set; }
        public int lowConfidence { get; set; }
        public double avgConfidence { get; set; }
        public int orders { get; set; }
        public int quotations { get; set; }
        public int receipts { get; set; }
        public int others { get; set; }
        public int errors { get; set; }
        public int duplicates { get; set; }
        public int certificates { get; set; }
        public int spams { get; set; }
        public int resolved { get; set; }
        public int unresolved { get; set; }
        public int unresolved_quotations { get; set; }
        public int total_replies_masterferro { get; set; }
        public int total_replies_client { get; set; }
        public int total_only_dates { get; set; }
    }

    public class FilteringStatisticsItemBuilder
    {
        private readonly FilteringStatisticsItem _filteringStatisticsItem = new();

        public FilteringStatisticsItemBuilder SetTotal(int total)
        {
            _filteringStatisticsItem.total = total;
            return this;
        }

        public FilteringStatisticsItemBuilder SetAutomatic(int automatic)
        {
            _filteringStatisticsItem.automatic = automatic;
            return this;
        }

        public FilteringStatisticsItemBuilder SetManual(int manual)
        {
            _filteringStatisticsItem.manual = manual;
            return this;
        }

        public FilteringStatisticsItemBuilder SetToValidate(int toValidate)
        {
            _filteringStatisticsItem.toValidate = toValidate;
            return this;
        }

        public FilteringStatisticsItemBuilder SetError(int error)
        {
            _filteringStatisticsItem.error = error;
            return this;
        }

        public FilteringStatisticsItemBuilder SetLowConfidence(int lowConfidence)
        {
            _filteringStatisticsItem.lowConfidence = lowConfidence;
            return this;
        }

        public FilteringStatisticsItemBuilder SetAvgConfidence(double avgConfidence)
        {
            _filteringStatisticsItem.avgConfidence = avgConfidence;
            return this;
        }

        public FilteringStatisticsItemBuilder SetOrders(int orders)
        {
            _filteringStatisticsItem.orders = orders;
            return this;
        }


        public FilteringStatisticsItemBuilder SetQuotations(int quotations)
        {
            _filteringStatisticsItem.quotations = quotations;
            return this;
        }

        public FilteringStatisticsItemBuilder SetReceipts(int receipts)
        {
            _filteringStatisticsItem.receipts = receipts;
            return this;
        }

        public FilteringStatisticsItemBuilder SetOthers(int others)
        {
            _filteringStatisticsItem.others = others;
            return this;
        }

        public FilteringStatisticsItemBuilder SetErrors(int errors)
        {
            _filteringStatisticsItem.errors = errors;
            return this;
        }

        public FilteringStatisticsItemBuilder SetDuplicates(int duplicates)
        {
            _filteringStatisticsItem.duplicates = duplicates;
            return this;
        }

        public FilteringStatisticsItemBuilder SetCertificates(int certificates)
        {
            _filteringStatisticsItem.certificates = certificates;
            return this;
        }

        public FilteringStatisticsItemBuilder SetSpams(int spams)
        {
            _filteringStatisticsItem.spams = spams;
            return this;
        }

        public FilteringStatisticsItemBuilder SetResolved(int resolved)
        {
            _filteringStatisticsItem.resolved = resolved;
            return this;
        }

        public FilteringStatisticsItemBuilder SetUnresolved(int unresolved)
        {
            _filteringStatisticsItem.unresolved = unresolved;
            return this;
        }

        public FilteringStatisticsItemBuilder SetUnresolvedQuotations(int unresolved_quotations)
        {
            _filteringStatisticsItem.unresolved_quotations = unresolved_quotations;
            return this;
        }

        public FilteringStatisticsItemBuilder SetTotalRepliesMasterferro(int total_replies_masterferro)
        {
            _filteringStatisticsItem.total_replies_masterferro = total_replies_masterferro;
            return this;
        }

        public FilteringStatisticsItemBuilder SetTotalRepliesClient(int total_client_replies)
        {
            _filteringStatisticsItem.total_replies_client = total_client_replies;
            return this;
        }

        public FilteringStatisticsItemBuilder SetTotalOnlyDates(int total_only_dates)
        {
            _filteringStatisticsItem.total_only_dates = total_only_dates;
            return this;
        }

        public FilteringStatisticsItem Build()
        {
            return _filteringStatisticsItem;
        }

    }
}
