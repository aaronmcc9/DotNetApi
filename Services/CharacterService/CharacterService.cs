using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public IHttpContextAccessor _httpContextAccessor { get; }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));
        public CharacterService(IMapper mapper, DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _mapper = mapper;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbCharacters = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => c.User.Id == GetUserId())
                    .ToListAsync();

                serviceResponse.Data = dbCharacters.Select(m => _mapper.Map<GetCharacterDto>(m)).ToList();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Could not fetch Characters";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error fetching character";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var userId = GetUserId();

            try
            {
                Character character = _mapper.Map<Character>(newCharacter);
                character.User = _dataContext.Users.FirstOrDefault(u => u.Id == userId);

                _dataContext.Characters.Add(character);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = await _dataContext.Characters
                    .Where(c => c.User.Id == userId)
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
                var userId = GetUserId();

                var character = await _dataContext.Characters
                    // .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User.Id == userId);

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
                    response.Message = "Character not found or user does not have access";
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
                var userId = GetUserId();
                var character = await _dataContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == userId);

                if (character != null)
                {
                    _dataContext.Characters.Remove(character);
                    await _dataContext.SaveChangesAsync();

                    response.Data = await _dataContext.Characters
                        .Where(c => c.User.Id == userId)
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found or user does not have access";
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                    c.User.Id == GetUserId());

                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found or user in not authorized";
                    return serviceResponse;
                }

                var skill = await _dataContext.Skills
                    .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill not found";
                    return serviceResponse;
                }

                character.Skills.Add(skill);
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