using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bukva
{
    class CustomLanguageConfiguration : LanguageConfiguration
    {
        public CustomLanguageConfiguration()
        {
            translationTable = new Dictionary<string, string>();
        }
    }
}
