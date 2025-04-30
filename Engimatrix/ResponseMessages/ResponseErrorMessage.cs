// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ResponseMessages
{
        /// <summary>
        /// Class with response error message codes.
        /// </summary>
        public class ResponseErrorMessage : ResponseMessage
        {
#pragma warning disable SA1600
                public const int InvalidArgs = -1;
                public const int InternalError = -2;
                public const int UserExists = -3;
                public const int UserRegistred = -4;
                public const int UnableToRegist = -5;
                public const int UnableToLogin = -6;
                public const int InvalidLogin = -7;
                public const int UnableToRefreshToken = -8;
                public const int UnableToRemoveUser = -9;
                public const int UserNotExists = -10;
                public const int UnableToRemoveAdmin = -11;
                public const int UnableToUpdateUser = -12;
                public const int UnableToUpdateAdmin = -13;
                public const int UnableToRegistEmployee = -14;
                public const int EmployeeExists = -15;
                public const int UnableToRemoveEmployee = -16;
                public const int EmployeeNotExists = -17;
                public const int UnableToUpdateEmployee = -18;
                public const int EmployeeProfileNotExists = -19;
                public const int UnableToUpdateEmployeeProfile = -20;
                public const int UnableToUploadFirm = -21;
                public const int ResponsibleFirmNotExists = -22;
                public const int UnableToUploadRhn = -23;
                public const int UnableToUploadCv = -24;
                public const int MapEntryNotExists = -25;
                public const int MapEntryAlreadyExists = -26;
                public const int PasswordExpired = -27;
                public const int InvalidArgsResetPass = -28;
                public const int EqualsRecentHistoryPass = -29;
                public const int UnableToUpdateReceipt = -30;
                public const int UnableToRemoveReceipt = -31;
                public const int ReceiptNotExists = -32;
                public const int UnableToRegistReceipt = -33;
                public const int ReceiptExists = -34;
                public const int UnableToFirmFile = -35;
                public const int FirmFileNotExists = -36;
                public const int ReceiptMonthYearNotExists = -37;
                public const int UnableToUpdatePassword = 38;
                public const int EqualsCurrentPass = -39;
                public const int PassNotEqualtoDB = -40;
                public const int UnableToCreateMapEntryBeginContract = -41;
                public const int ErrorCreateCSV = -42;
                public const int UnableToRemoveScript = -43;
                public const int UnableToEditScript = -44;
                public const int UnableToStartScript = -45;
                public const int ScriptError = -46;
                public const int UnableToUpdateScriptStatus = -47;
                public const int QueueError = -48;
                public const int ErrorAddingQueue = -49;
                public const int ErrorRemovingQueue = -50;
                public const int ErrorEditingQueue = -51;
                public const int ErrorAddingTransaction = -52;
                public const int ErrorRemovingTransaction = -53;
                public const int ErrorEditingTransaction = -54;
                public const int JobsError = -55;
                public const int AssetsError = -56;
                public const int ErrorAddingAsset = -57;
                public const int ErrorEditingAsset = -58;
                public const int ErrorRemovingAsset = -59;
                public const int ErrorEmailTokenNotFoundInConcurrency = -60;
                public const int AnotherUserIsOwnerOfConcurrency = -61;
                public const int EmailNotFound = -62;
                public const int InvalidTableName = -63;
                public const int InvalidRating = -64;
                public const int ErrorSendingEmail = -65;
                public const int ErrorSavingEmail = -66;
                public const int ErrorSavingAttachment = -67;
                public const int AlreadyLoading = -68;
                public const int NotFound = -69;
                public const int DatabaseQueryError = -70;
                public const int ResourceEmpty = -71;
                public const int PrimaveraApiError = -72;
                public const int EmailProcessing = -73;
                public const int ProductInvalidStock = -74;
                public const int ItemAlreadyExists = -75;
                public const int OnlyAdmins = -76;
                public const int TriggersError = -77;
                public const int ErrorEditingTrigger = -78;
                public const int ErrorAddingTrigger = -79;
                public const int ErrorRemovingTrigger = -80;
                public const int Unauthorized = -81;

#pragma warning restore SA1600
        }
}
