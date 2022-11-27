using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SkillController: ControllerBase
    {
        ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }
    }
}