using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Fight;

namespace Services
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);

    }
}