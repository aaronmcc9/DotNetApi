using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Character;
using Dto.Weapon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.WeaponService;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;

        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await this._weaponService.AddWeapon(newWeapon));
        }
    }
}