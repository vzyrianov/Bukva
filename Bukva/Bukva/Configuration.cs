using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bukva
{
    public class Configuration
    {
        public List<KeyTranslation> keyTranslations { get; set; }
        public List<Shortcut> shortcuts { get; set; }
    }
}