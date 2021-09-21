using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _mapper = mapper;
            _npRepo = npRepo;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto); 
        }
    }
}
