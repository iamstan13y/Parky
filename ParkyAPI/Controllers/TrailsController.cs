using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public class TrailsController : ControllerBase
        {
            private readonly ITrailRepository _trailRepo;
            private readonly IMapper _mapper;

            public TrailsController(ITrailRepository trialRepo, IMapper mapper)
            {
                _mapper = mapper;
                _trailRepo = trialRepo;
            }

            /// <summary>
            /// Get list of Trails.
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
            public IActionResult GetTrails()
            {
            var objList = _trailRepo.GetTrails();

                var objDto = new List<TrailDto>();

                foreach (var obj in objList)
                {
                    objDto.Add(_mapper.Map<TrailDto>(obj));
                }
                return Ok(objDto);
            }

            /// <summary>
            /// Get Individual Trail.
            /// </summary>
            /// <param name="id">The Id of the Trail.</param>
            /// <returns></returns>
            [HttpGet("{id:int}", Name = "GetTrail")]
            [ProducesResponseType(200, Type = typeof(TrailDto))]
            [ProducesResponseType(404)]
            [ProducesDefaultResponseType]
            [Authorize(Roles = "Admin")]
            public IActionResult GetTrail(int id)
            {
                var obj = _trailRepo.GetTrail(id);
                if (obj == null)
                {
                    return NotFound();
                }
                var objDto = _mapper.Map<TrailDto>(obj);

                return Ok(objDto);
            }

        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpPost]
            [ProducesResponseType(201, Type = typeof(TrailDto))]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesDefaultResponseType]
            public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
            {
                if (trailDto == null)
                {
                    return BadRequest(ModelState);
                }

                if (_trailRepo.TrailExists(trailDto.Name))
                {
                    ModelState.AddModelError("", "Trail Exists!");
                    return StatusCode(404, ModelState);
                }

                var trailObj = _mapper.Map<Trail>(trailDto);

                if (!_trailRepo.CreateTrail(trailObj))
                {
                    ModelState.AddModelError("", $"Something went wrong creating record: {trailObj.Name}!");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("GetTrail", new { id = trailObj.Id }, trailObj);
            }

            [HttpPatch("{id:int}", Name = "UpdateTrail")]
            [ProducesResponseType(204)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
            {
                if (trailDto == null || id != trailDto.Id)
                {
                    return BadRequest(ModelState);
                }

                var trailObj = _mapper.Map<Trail>(trailDto);

                if (!_trailRepo.UpdateTrail(trailObj))
                {
                    ModelState.AddModelError("", $"Something went wrong updating record: {trailObj.Name}!");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }

            [HttpDelete("{id:int}", Name = "DeleteTrail")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult DeleteTrail(int id)
            {
                if (!_trailRepo.TrailExists(id))
                {
                    return NotFound();
                }

                var trailObj = _trailRepo.GetTrail(id);

                if (!_trailRepo.DeleteTrail(trailObj))
                {
                    ModelState.AddModelError("", $"Something went wrong deleting record: {trailObj.Name}!");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
        }
}
