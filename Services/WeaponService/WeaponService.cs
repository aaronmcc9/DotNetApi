using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Dto.Character;
using Dto.Weapon;
using Microsoft.EntityFrameworkCore;

namespace Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private DataContext _dataContext { get; }
        private IHttpContextAccessor _httpContextAccessor { get; }
        private IMapper _mapper { get; }
        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = context;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                Character character = await _dataContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                        c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirstValue(ClaimTypes.NameIdentifier)));

                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                    return serviceResponse;
                }

                Weapon weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                // character.Weapon = weapon;

                _dataContext.Weapons.Add(weapon);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
    }
}