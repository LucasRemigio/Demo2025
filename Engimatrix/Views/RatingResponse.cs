// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class RatingCriteriaDTOListResponse
    {
        public List<RatingCriteriaDTO> ratings { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingCriteriaDTOListResponse(List<RatingCriteriaDTO> ratings, int result_code, string language)
        {
            this.ratings = ratings;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingCriteriaDTOListResponse(int result_code, string language)
        {
            this.ratings = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class RatingCriteriaDTOResponse
    {
        public RatingCriteriaDTO rating { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingCriteriaDTOResponse(RatingCriteriaDTO rating, int result_code, string language)
        {
            this.rating = rating;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingCriteriaDTOResponse(int result_code, string language)
        {
            this.rating = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class RatingDiscountListResponse
    {
        public List<RatingDiscountItem> rating_discounts { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingDiscountListResponse(List<RatingDiscountItem> ratingDiscounts, int result_code, string language)
        {
            this.rating_discounts = ratingDiscounts;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingDiscountListResponse(int result_code, string language)
        {
            this.rating_discounts = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class RatingDiscountItemResponse
    {
        public RatingDiscountItem rating_discount { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingDiscountItemResponse(RatingDiscountItem ratingDiscount, int result_code, string language)
        {
            this.rating_discount = ratingDiscount;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingDiscountItemResponse(int result_code, string language)
        {
            this.rating_discount = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }


    public class RatingTypeListResponse
    {
        public List<RatingTypeItem> rating_types { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingTypeListResponse(List<RatingTypeItem> ratingTypes, int result_code, string language)
        {
            this.rating_types = ratingTypes;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingTypeListResponse(int result_code, string language)
        {
            this.rating_types = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class RatingTypeItemResponse
    {
        public RatingTypeItem rating_type { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public RatingTypeItemResponse(RatingTypeItem ratingType, int result_code, string language)
        {
            this.rating_type = ratingType;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public RatingTypeItemResponse(int result_code, string language)
        {
            this.rating_type = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
