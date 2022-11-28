using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dto.Fight
{
    public class AttackResultDto
    {
        public string AttackerName { get; set; }
        public string OpponentName { get; set; }
        public int AttackerHitPoints { get; set; }
        public int OpponentHitPoints { get; set; }
        public int Damage { get; set; }

    }
}