// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Drawing;

namespace engimatrix.Utils.QRCode
{
    public class QRCodeResult
    {
        /// <summary>
        /// QR Code Data array
        /// </summary>
        public byte[] DataArray;

        /// <summary>
        /// ECI Assignment Value
        /// </summary>
        public int ECIAssignValue;

        /// <summary>
        /// QR Code matrix version
        /// </summary>
        public int QRCodeVersion;

        /// <summary>
        /// QR Code matrix dimension in bits
        /// </summary>
        public int QRCodeDimension;

        public Rectangle Bounds;

        /// <summary>
        /// QR Code error correction code (L, M, Q, H)
        /// </summary>
        public ErrorCorrection ErrorCorrection;

        public QRCodeResult(byte[] DataArray)
        {
            this.DataArray = DataArray;
            return;
        }
    }
}
