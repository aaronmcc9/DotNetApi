using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Character;

namespace _Net_Course
{
    public class AutoMapperProfile: AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, GetCharacterDto>();    
        }
    }
}