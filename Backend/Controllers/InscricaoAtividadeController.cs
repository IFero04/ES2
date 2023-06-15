using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricaoAtividadeController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public InscricaoAtividadeController(ES2DBContext context)
        {
            _context = context;
        }

        // GET: api/IncricaoAtividade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscricaoAtividade>>> GetInscricaoAtividades()
        {
          if (_context.InscricaoAtividades == null)
          {
              return NotFound();
          }
            return await _context.InscricaoAtividades.ToListAsync();
        }

        // GET: api/IncricaoAtividade/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InscricaoAtividade>> GetInscricaoAtividade(Guid id)
        {
          if (_context.InscricaoAtividades == null)
          {
              return NotFound();
          }
            var inscricaoAtividade = await _context.InscricaoAtividades.FindAsync(id);

            if (inscricaoAtividade == null)
            {
                return NotFound();
            }

            return inscricaoAtividade;
        }

        // PUT: api/IncricaoAtividade/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricaoAtividade(Guid id, InscricaoAtividade inscricaoAtividade)
        {
            if (id != inscricaoAtividade.Id)
            {
                return BadRequest();
            }

            _context.Entry(inscricaoAtividade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoAtividadeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IncricaoAtividade
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InscricaoAtividade>> PostInscricaoAtividade(CreateInscricaoAtividadeModel model)
        {
            var inscricaoAtividade = new InscricaoAtividade()
            {
                IdAtividade = model.IdAtividade,
                IdParticipante = model.IdParticipante
            };
                
            _context.InscricaoAtividades.Add(inscricaoAtividade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscricaoAtividade", new { id = inscricaoAtividade.Id }, inscricaoAtividade);
        }

        // DELETE: api/IncricaoAtividade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscricaoAtividade(Guid id)
        {
            if (_context.InscricaoAtividades == null)
            {
                return NotFound();
            }
            var inscricaoAtividade = await _context.InscricaoAtividades.FindAsync(id);
            if (inscricaoAtividade == null)
            {
                return NotFound();
            }

            _context.InscricaoAtividades.Remove(inscricaoAtividade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InscricaoAtividadeExists(Guid id)
        {
            return (_context.InscricaoAtividades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
