using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bukva
{
    class LanguageConfiguration
    {
        protected Dictionary<KeyPressChord, string> translationTable;

        public LanguageConfiguration()
        {
            translationTable = new Dictionary<KeyPressChord, string>();
        }


    }
}
