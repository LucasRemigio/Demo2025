// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public interface IRatingItem
    {
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public DateTime? updated_at { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public string? created_by { get; set; }
    }
}
