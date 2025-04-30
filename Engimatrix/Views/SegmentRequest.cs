// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class SegmentCreateRequest
    {
        public required string name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name);
        }
    }

    public class SegmentUpdateRequest
    {
        public required string name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
