using System;
using System.Collections.Generic;

namespace CLUBS.Localization
{
    public class Language
    {
        public Dictionary<string, string> RawData = new Dictionary<string, string>();
        public static Language CurrentLanguage= new Language();
        public Language()
        {
        
        }
    }
}
