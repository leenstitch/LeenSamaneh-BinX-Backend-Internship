using LensBook.Dto_s.SessionType;
using LensBook.Dto_s.SessionTypeDto_s;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LensBook.Controllers
{
    [ApiController]
    [Route("api/v1/SessionTypes")]
    public class SessionTypesController : ControllerBase
    {
        private readonly ISessionTypeService _service;

        public SessionTypesController(
            ISessionTypeService service)
        {
            _service = service;
        }

        
        // CREATE SESSION TYPE

        [HttpPost]
        [Authorize(Roles = "StudioOwner")]
        public async Task<IActionResult> Create(
            CreateSessionTypeDto dto)
        {
            var result =
                await _service.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }

        
        // GET session type BY ID

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Session type not found."
                    });
            }

            return Ok(result);
        }


        // GET ALL session types

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _service.GetAllAsync();

            return Ok(result);
        }
    }
}