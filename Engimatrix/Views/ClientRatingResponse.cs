// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ClientRatingListResponse
    {
        public List<ClientRatingDTO> client_ratings { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ClientRatingListResponse(List<ClientRatingDTO> clientRatings, int result_code, string language)
        {
            this.client_ratings = clientRatings;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ClientRatingListResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class ClientRatingItemResponse
    {
        public ClientRatingDTO client_rating { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public ClientRatingItemResponse(ClientRatingDTO clientRating, int result_code, string language)
        {
            this.client_rating = clientRating;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ClientRatingItemResponse(int result_code, string language)
        {
            this.client_rating = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
