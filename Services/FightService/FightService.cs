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

                int damage = DoWeaponAttack(attacker, opponent);

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

        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
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
                int damage = DoSkillAttack(attacker, opponent, skill);

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

        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            ServiceResponse<FightResultDto> serviceResponse = new ServiceResponse<FightResultDto>{
                Data = new FightResultDto()
            };

            try
            {
                var characters = _dataContext.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id))
                    .ToList();

                bool defeated = false;

                while (!defeated)
                {
                    //in order so each character takes turns attacking
                    foreach (Character attacker in characters)
                    {
                        var opponents = characters
                            .Where(c => c.Id != attacker.Id)
                            .ToList();

                        var opponent = opponents[new Random().Next(opponents.Count())];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count())];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }

                        serviceResponse.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage.");

                        if (opponent.HitPoints <= 0)
                        {
                            attacker.Victories++;
                            opponent.Defeats--;
                            defeated = true;

                            serviceResponse.Data.Log.Add($"{opponent.Name} has been defeated!");
                            serviceResponse.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} left!");
                        }
                    }

                    //RESET
                    characters.ForEach(c =>
                    {
                        c.Fights++;
                        c.HitPoints = 100;
                    });

                    await _dataContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighScore()
        {
            ServiceResponse<List<HighscoreDto>> serviceResponse = new ServiceResponse<List<HighscoreDto>>();
            try
            {
                var characters = await _dataContext.Characters
                    .Where(c => c.Fights > 0)
                    .OrderByDescending(c => c.Victories)
                    .ThenBy(c => c.Defeats)
                    .ToListAsync();

                serviceResponse.Data = characters
                    .Select(c => _mapper
                        .Map<HighscoreDto>(c))
                    .ToList();
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