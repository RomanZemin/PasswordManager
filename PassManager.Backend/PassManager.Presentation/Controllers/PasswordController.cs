using Microsoft.AspNetCore.Mvc;
using PassManager.Application.Interfaces;
using PassManager.Domain.Entities;

namespace PassManager.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordRepository _repository;

        public PasswordController(IPasswordRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a list of all password records.
        /// </summary>
        /// <returns>A list of password records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PasswordRecord>>> GetPasswords()
        {
            var passwords = await _repository.GetPasswordsAsync();
            return Ok(passwords);
        }

        /// <summary>
        /// Adds a new password record.
        /// </summary>
        /// <param name="passwordRecord">The password record to add.</param>
        /// <returns>The created password record.</returns>
        [HttpPost]
        public async Task<ActionResult<PasswordRecord>> AddPassword(PasswordRecord passwordRecord)
        {
            if (await _repository.PasswordExistsAsync(passwordRecord.Name))
            {
                return Conflict("Record with the same name already exists.");
            }

            passwordRecord.CreatedAt = DateTime.Now;
            await _repository.AddPasswordAsync(passwordRecord);

            return CreatedAtAction(nameof(GetPasswords), new { id = passwordRecord.Id }, passwordRecord);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassword(int id)
        {
            var existingRecord = await _repository.GetPasswordByIdAsync(id);
            if (existingRecord == null)
            {
                return NotFound();
            }

            await _repository.DeletePasswordAsync(id);
            return NoContent();
        }
    }
}