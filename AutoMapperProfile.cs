using Dto.Character;

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
        }
    }
}