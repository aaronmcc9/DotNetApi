using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Fight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await _fightService.WeaponAttack(request));
        }

        [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            return Ok(await _fightService.SkillAttack(request));
        }
    }
}