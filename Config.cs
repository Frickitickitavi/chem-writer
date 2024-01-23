using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemWriter
{
    internal class Config
    {
        public const int MIN_THEORETICAL = 119;
        public const int MAX_ATTEMPTED = 127;
        public const int MAX_THEORETICAL = 173;
        public const int MAX_REAL = MIN_THEORETICAL - 1;
        public const int MIN_OLD_PROCEDURAL = 104;
        public const int MIN_OLD_TRIVIAL = 101;

        public Config(UInt64 min, UInt64 max)
        {
            minSystematic = min;
            maxSystematic = max;
        }

        public UInt64 minSystematic { get; private set; }
        public UInt64 maxSystematic { get; private set; }

        public static bool DEBUG = false;
    }
}
