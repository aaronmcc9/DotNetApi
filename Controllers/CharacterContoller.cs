using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto.Character;
using Microsoft.AspNetCore.Mvc;
using Services.CharacterService;

namespace Controllers
{
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
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id){
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter){
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
    }
}