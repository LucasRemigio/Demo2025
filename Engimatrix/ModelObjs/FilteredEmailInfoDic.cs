// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class ReplyInfo
    {
        public int ReplyCount { get; set; }
        public int UnreadReplyCount { get; set; }
        public DateTime LastRepliedAt { get; set; }
        public string LastRepliedBy { get; set; }

        public ReplyInfo()
        {
        }

        public ReplyInfo(int ReplyCount, int UnreadReplyCount, DateTime LastRepliedAt, string LastRepliedBy)
        {
            this.ReplyCount = ReplyCount;
            this.UnreadReplyCount = UnreadReplyCount;
            this.LastRepliedAt = LastRepliedAt;
            this.LastRepliedBy = LastRepliedBy;
        }
    }

    public class ForwardInfo
    {
        public int ForwardCount { get; set; }
        public DateTime LastForwardedAt { get; set; }
        public string LastForwardedBy { get; set; }

        public ForwardInfo()
        {
        }

        public ForwardInfo(int ForwardCount, DateTime LastForwardedAt, string LastForwardedBy)
        {
            this.ForwardCount = ForwardCount;
            this.LastForwardedAt = LastForwardedAt;
            this.LastForwardedBy = LastForwardedBy;
        }
    }
}
