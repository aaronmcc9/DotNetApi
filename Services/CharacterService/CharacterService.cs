using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Dto.Character;
using Microsoft.EntityFrameworkCore;

namespace Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        public IMapper _mapper { get; }
        private readonly DataContext _dataContext;

        public CharacterService(IMapper mapper, DataContext dataContext)
        {
            this._dataContext = dataContext;
            this._mapper = mapper;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _dataContext.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(m => _mapper.Map<GetCharacterDto>(m)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            try
            {
                _dataContext.Characters.Add(character);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = await _dataContext.Characters
                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _dataContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character != null)
                {
                    _mapper.Map(updatedCharacter, character);

                    _dataContext.Characters.Update(character);
                    await _dataContext.SaveChangesAsync();

                    response.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found";
                }

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }


            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (character != null)
                {
                    _dataContext.Characters.Remove(character);
                    await _dataContext.SaveChangesAsync();

                    response.Data = await _dataContext.Characters
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found";
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }
    }
}