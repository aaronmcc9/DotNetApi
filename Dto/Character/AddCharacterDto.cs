using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dto.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; }

        public int HitPoints { get; set; }

        public int Strength { get; set; }

        public int Defence { get; set; }

        public int Intelligence { get; set; }

        public RpgClass Class  { get; set; }
    }
}