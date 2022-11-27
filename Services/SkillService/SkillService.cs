using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;

namespace Services.SkillService
{
    public class SkillService : ISkillService
    {
        public DataContext _dataContext;
        public IMapper _mapper { get; }
        public SkillService(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }
    }
}