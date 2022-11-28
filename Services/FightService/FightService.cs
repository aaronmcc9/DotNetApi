using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Dto.Fight;
using Microsoft.EntityFrameworkCore;

namespace Services.FightService
{
    public class FightService : IFightService
    {
        public DataContext _dataContext;
        public IMapper _mapper { get; }
        public FightService(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            ServiceResponse<AttackResultDto> serviceResponse = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker == null || opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character(s) not found";
                    return serviceResponse;
                }

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defence);

                if (damage > 0)
                    opponent.HitPoints -= damage;

                if (opponent.HitPoints <= 0)
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto
                {
                    AttackerName = attacker.Name,
                    OpponentName = opponent.Name,
                    AttackerHitPoints = attacker.HitPoints,
                    OpponentHitPoints = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            ServiceResponse<AttackResultDto> serviceResponse = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _dataContext.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _dataContext.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker == null || opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character(s) not found";
                    return serviceResponse;
                }

                var skill = attacker.Skills
                    .FirstOrDefault(s => s.Id == request.SkillId);

                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{attacker.Name} does not know that skill";
                    return serviceResponse;
                }
                int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
                damage -= new Random().Next(opponent.Defence);

                if (damage > 0)
                    opponent.HitPoints -= damage;

                if (opponent.HitPoints <= 0)
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto
                {
                    AttackerName = attacker.Name,
                    OpponentName = opponent.Name,
                    AttackerHitPoints = attacker.HitPoints,
                    OpponentHitPoints = opponent.HitPoints,
                    Damage = damage
                };
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