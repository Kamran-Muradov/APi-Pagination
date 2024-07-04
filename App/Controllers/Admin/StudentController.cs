using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.GroupStudents;
using Service.DTOs.Admin.Students;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto request)
        {
            await _studentService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { Response = "Succesfull" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] StudentEditDto request)
        {
            await _studentService.EditAsync(id, request);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _studentService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupStudentCreateDto request)
        {
            await _studentService.AddGroupAsync(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup([FromQuery] int id)
        {
            await _studentService.DeleteGroupAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mappedStudents = await _studentService.GetAllWithInclude();
            return Ok(mappedStudents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _studentService.GetByIdAsync(id));
        }
    }
}
