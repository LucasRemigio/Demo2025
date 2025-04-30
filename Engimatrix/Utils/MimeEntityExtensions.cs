// // Copyright (c) 2024 Engibots. All rights reserved.

using MimeKit;

namespace Engimatrix.Utils
{
    public static class MimeEntityExtensions
    {
        public static string ConvertToBase64String(this MimeEntity attachment)
        {
            using var memoryStream = new MemoryStream();
            if (attachment is MimePart mimePart)
            {
                mimePart.Content.DecodeTo(memoryStream);
            }
            else if (attachment is MessagePart messagePart)
            {
                messagePart.Message.WriteTo(memoryStream);
            }
            else
            {
                throw new InvalidOperationException("Unsupported MimeEntity type");
            }

            byte[] attachmentBytes = memoryStream.ToArray();
            return Convert.ToBase64String(attachmentBytes);
        }
    }
}
