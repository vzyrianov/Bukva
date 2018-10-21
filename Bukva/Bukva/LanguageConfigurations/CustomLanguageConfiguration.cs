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

        public void LoadConfiguration(List<KeyTranslation> keyTranslations)
        {
            foreach(KeyTranslation keyTranslation in keyTranslations) {
                string keyPressChord = "";

                foreach(KeyPress keyPress in keyTranslation.input)
                {
                    if (keyPress.isShiftDown)
                        keyPressChord += keyPress.key.ToUpper();
                    else
                        keyPressChord += keyPress.key.ToLower();
                }

                translationTable.Add(keyPressChord, keyTranslation.output);
            }
        }
    }
}
