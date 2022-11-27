using Dto.Character;
using Dto.Skill;
using Dto.Weapon;

namespace _Net_Course
{
    public class AutoMapperProfile: AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();    
            CreateMap<UpdateCharacterDto, Character>();    
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}