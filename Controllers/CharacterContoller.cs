using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dto.Character;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CharacterService;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterContoller : ControllerBase
    {
        ICharacterService _characterService;


        public CharacterContoller(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var response = await _characterService.AddCharacter(newCharacter);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

         [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacter)
        {
            var response = await _characterService.AddCharacterSkill(newCharacter);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }
    }
}