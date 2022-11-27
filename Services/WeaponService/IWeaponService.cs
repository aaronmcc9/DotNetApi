using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Character;
using Dto.Weapon;

namespace Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}