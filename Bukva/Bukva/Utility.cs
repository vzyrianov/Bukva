using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bukva
{
    public class Input
    {
        public string key { get; set; }
        public bool isShiftDown { get; set; }
        public bool isCtrlDown { get; set; }
    }

    public class KeyTranslation
    {
        public string output { get; set; }
        public List<Input> input { get; set; }
    }

    public class Bind
    {
        public string key { get; set; }
        public bool isShiftDown { get; set; }
        public bool isCtrlDown { get; set; }
    }

    public class Shortcut
    {
        public string commandName { get; set; }
        public Bind bind { get; set; }
    }

}
