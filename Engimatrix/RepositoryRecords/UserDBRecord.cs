// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    using engimatrix.ModelObjs;

    public class UserDBRecord
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRoleId { get; set; }
        public string ActiveSince { get; set; }
        public string LastLogin { get; set; }
        public string Language { get; set; }
        public string PassExpires { get; set; }
        public string LastPassChanges { get; set; }
        public string PassHistory { get; set; }

        public UserDBRecord(string userId, string userEmail, string userName, string userPassword, string userRoleId, string activeSince, string lastLogin,
            string language, string passExpires, string lastPassChange, string passHistory)
        {
            this.UserId = userId;
            this.UserEmail = userEmail;
            this.UserName = userName;
            this.UserPassword = userPassword;
            this.UserRoleId = userRoleId;
            this.ActiveSince = activeSince;
            this.LastLogin = lastLogin;
            this.Language = language;
            this.PassExpires = passExpires;
            this.LastPassChanges = lastPassChange;
            this.PassHistory = passHistory;
        }
    }
}
