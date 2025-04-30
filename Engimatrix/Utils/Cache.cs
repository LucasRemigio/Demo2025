// // Copyright (c) 2024 Engibots. All rights reserved.

using MessagePack;
using engimatrix.ModelObjs;

namespace engimatrix.Utils
{
    public class Cache
    {
        public static class OrderCache
        {
            private static Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>();
            private static string DataFolderPath = "Data";
            private static string CacheFileName = "full_cache";

            public static string GetCachedOrder(string key)
            {
                string filePath = GetCacheFilePath();
                if (cache.TryGetValue(key, out CacheEntry cacheEntry))
                {
                    cacheEntry.LastAccessed = DateTime.UtcNow;
                    return cacheEntry.Value;
                }
                return null;
            }

            private static int counter = 0;

            public static string GetCachedOrderEncrypt(string key, string value)
            {
                int limit = 10000000;

                while (counter <= limit)
                {
                    string data = GetDataFromSource(key, value);
                    cache[key] = new CacheEntry { Value = data, LastAccessed = DateTime.UtcNow };
                    counter++;
                    return data;
                }
                return null;
            }

            private static string GetDataFromSource(string key, string value)
            {
                return $"{value}";
            }

            private static string GetCacheFilePath()
            {
                return Path.Combine(DataFolderPath, CacheFileName);
            }

            public static void SaveDictionaryToBinaryFile(Dictionary<string, CacheEntry> data, string filePath)
            {
                byte[] bytes = MessagePackSerializer.Serialize(data);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }

            public static void InitializeCache()
            {
                try
                {
                    string filePath = GetCacheFilePath();

                    RemoveInactiveKeys();

                    if (File.Exists(filePath))
                    {
                        cache = LoadData(filePath);
                    }
                    else
                    {
                        Log.Error($"Binary file not found at: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred in InitializeCache: {ex.Message}");
                }
            }

            private static void RemoveInactiveKeys()
            {
                DateTime cutoffDate = DateTime.UtcNow.AddMonths(-3);

                var cachedData = LoadData(GetCacheFilePath());
                var keysToRemove = cachedData
                    .Where(kv => kv.Value.LastAccessed < cutoffDate)
                    .Select(kv => kv.Key)
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    cachedData.Remove(key);
                }

                SaveDictionaryToBinaryFile(cachedData, GetCacheFilePath());
            }

            public static void SaveCacheOnShutdown()
            {
                AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
                {
                    try
                    {
                        string filePath = GetCacheFilePath();
                        SaveDictionaryToBinaryFile(cache, filePath);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"An error occurred while saving cache on shutdown: {ex.Message}");
                    }
                };
            }

            private static Dictionary<string, CacheEntry> LoadData(string filePath)
            {
                byte[] rawData;

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    rawData = new byte[fileStream.Length];
                    fileStream.Read(rawData, 0, rawData.Length);
                }

                return MessagePackSerializer.Deserialize<Dictionary<string, CacheEntry>>(rawData);
            }
        }
    }
}
