using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bukva
{
    interface IKeyTranslator
    {
        void Start();
        void Stop();
        void SetLetterTable(LetterTable newLetterTable);
    }
}
