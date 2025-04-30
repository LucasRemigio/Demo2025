// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ResponseMessages
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using engimatrix.Utils;
    using static engimatrix.ResponseMessages.ResponseMessageJSON;

    /// <summary>
    /// This class will handle response translations and codes, based on JSON files in the same folder.
    /// </summary>
    public abstract class ResponseMessage
    {
#pragma warning disable SA1600

        private const string DefaultLanguage = "en";
        private static Dictionary<string, Dictionary<int, string>> messages;
#pragma warning restore SA1600

        /// <summary>
        /// Get default response message for a given code.
        /// </summary>
        /// <param name="code">Response code.</param>
        /// <returns>Response string in default language.</returns>
        public static string GetResponseMessage(int code)
        {
            return messages[DefaultLanguage][code];
        }

        /// <summary>
        /// Get response message for a given code and language.
        /// </summary>
        /// <param name="code">Response code.</param>
        /// <param name="language">Language (ex. en, pt). It may come pt-PT or en-US and need to be parsed.</param>
        /// <returns>Response string in language provided as argument.</returns>
        public static string GetResponseMessage(int code, string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return messages[DefaultLanguage][code];
            }
            else if (language.Length != 2)
            {
                string[] parts = language.Split('-');
                if (parts.Length != 2)
                {
                    return messages[DefaultLanguage][code];
                }

                language = parts[0];
            }

            language = language.ToLower();
            if (messages.ContainsKey(language))
            {
                return messages[language][code];
            }

            return messages[DefaultLanguage][code];
        }

        /// <summary>
        /// Load to messages dictionary the content of JSON files in the same folder
        /// The dictionary will hold code a string organized by each language available on JSONs.
        /// </summary>
        public static void LoadResponseMessages()
        {
            Log.Debug("********************************************");
            Log.Debug("*** Loading response messages from jsons ***");
            messages = new Dictionary<string, Dictionary<int, string>>();

            DirectoryInfo d = new("ResponseMessages");
            foreach (var messagesFile in d.GetFiles("*.json"))
            {
                Log.Debug("Response messages file found: " + messagesFile.FullName);

                using StreamReader readerMessages = new(messagesFile.FullName);
                string jsonString = readerMessages.ReadToEnd();
                readerMessages.Close();
                ResponseMessageJSONArray fileResult = JsonConvert.DeserializeObject<ResponseMessageJSONArray>(jsonString);

                string language = Path.GetFileNameWithoutExtension(messagesFile.FullName).Replace("-error", string.Empty).Replace("-success", string.Empty);

                if (!messages.ContainsKey(language))
                {
                    messages.Add(language, new Dictionary<int, string>());
                }

                foreach (ResponseMessageJSONObject currItem in fileResult.Messages)
                {
                    messages[language].Add(currItem.Code, currItem.Message);
                }
            }
        }
    }
}
