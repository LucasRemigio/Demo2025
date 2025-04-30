// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Utils
{
    /// <summary>
    /// QR Code error correction code enumeration
    /// </summary>
    public enum ErrorCorrection
    {
        /// <summary>
        /// Low (01)
        /// </summary>
        L,

        /// <summary>
        /// Medium (00)
        /// </summary>
        M,

        /// <summary>
        /// Medium-high (11)
        /// </summary>
        Q,

        /// <summary>
        /// High (10)
        /// </summary>
        H,
    }
}
