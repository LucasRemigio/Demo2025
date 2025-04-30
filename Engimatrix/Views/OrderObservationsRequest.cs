// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class OrderObservationsRequest
    {
        public string? observations { get; set; }
        public string? contact { get; set; }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(contact))
            {
                if (contact.Length > 255)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(observations))
            {
                if (observations.Length > 1000)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
