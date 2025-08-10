using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Types
{
    [Flags]
    public enum SpellDescriptors
    {
        Acid = 0x00001,
        Air = 0x00002,
        Chaotic = 0x00004,
        Cold = 0x00008,
        Darkness = 0x00010,
        Death = 0x00020,
        Earth = 0x00040,
        Electricity = 0x00080,
        Evil = 0x00100,
        Fear = 0x00200,
        Fire = 0x00400,
        Force = 0x00800,
        Good = 0x01000,
        LanguageDependent = 0x02000,
        Lawful = 0x04000,
        Light = 0x08000,
        MindAffecting = 0x10000,
        Sonic = 0x20000,
        Water = 0x40000
    }
}
