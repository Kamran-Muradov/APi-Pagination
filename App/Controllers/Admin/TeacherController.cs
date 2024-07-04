using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Teachers;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{

    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherTeacherService)
        {
            _teacherService = teacherTeacherService;           
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _teacherService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _teacherService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateDto request)
        {
            await _teacherService.Create(request);
            return CreatedAtAction(nameof(Create), new {Response = "Data succesfully created"});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] TeacherEditDto request)
        {
            await _teacherService.EditAsync(id, request);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _teacherService.DeleteAsync(id);
            return Ok();
        }
    }
}
