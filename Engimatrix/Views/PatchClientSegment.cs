// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class PatchClientSegment
    {
        public int segment_id { get; set; }

        public bool IsValid()
        {
            if (this.segment_id <= 0 || this.segment_id > 1000)
            {
                return false;
            }

            return true;
        }
    }
}
