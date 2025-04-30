// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class LanguagesItem
    {
        public string language { get; set; }
        public string read { get; set; }
        public string speak { get; set; }
        public string write { get; set; }

        public LanguagesItem(string language, string read, string speak, string write)
        {
            this.language = language;
            this.read = read;
            this.speak = speak;
            this.write = write;
        }

        public override string ToString()
        {
            return this.language + "|" + this.read + "|" + this.speak + "|" + this.write;
        }
    }
}
