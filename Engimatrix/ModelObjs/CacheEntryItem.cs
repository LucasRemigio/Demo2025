// // Copyright (c) 2024 Engibots. All rights reserved.

using MessagePack;

namespace engimatrix.ModelObjs
{
    [MessagePackObject]
    public class CacheEntry
    {
        [Key(0)]
        public string Value { get; set; }

        [Key(1)]
        public DateTime LastAccessed { get; set; }
    }
}
