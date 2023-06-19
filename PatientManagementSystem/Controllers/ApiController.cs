using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Core.Models;
using PatientManagementSystem.Data;

namespace PatientManagementSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        [HttpGet("doctors/{doctorId}/patients")]
        //  [HttpGet("patients?doctorId=${doctorId}/patients")]
        public async Task<IActionResult> GetDoctorPatients(int doctorId)
        {
            var patients = await _context.Patients.Where(p => p.DoctorId == doctorId).ToListAsync();
            return Ok(patients);
        }

        [HttpPost("patients")]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("patients/{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }
    }
}