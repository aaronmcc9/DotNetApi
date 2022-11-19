using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dto.Character;

namespace Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character{
                Id = 1,
                Name = "Freddie",
                HitPoints = 100,
                Strength = 15,
                Defence = 6,
                Class = RpgClass.Mage
                 },
            new Character{
                Id = 2,
                Name = "Dan",
                HitPoints = 100,
                Strength = 20,
                Defence = 2,
                Class = RpgClass.Cleric
                 },
        };
        public IMapper _mapper { get; }

        public CharacterService(IMapper mapper)
        {
            this._mapper = mapper;
            
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(m => _mapper.Map<GetCharacterDto>(m)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);  
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter); 
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(m => _mapper.Map<GetCharacterDto>(m)).ToList();
            return serviceResponse;
        }

    }
}