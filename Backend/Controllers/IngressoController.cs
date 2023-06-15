using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngressoController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public IngressoController(ES2DBContext context)
        {
            _context = context;
        }

        // GET: api/Ingresso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingresso>>> GetIngressos()
        {
          if (_context.Ingressos == null)
          {
              return NotFound();
          }
            return await _context.Ingressos.ToListAsync();
        }

        // GET: api/Ingresso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingresso>> GetIngresso(Guid id)
        {
          if (_context.Ingressos == null)
          {
              return NotFound();
          }
            var ingresso = await _context.Ingressos.FindAsync(id);

            if (ingresso == null)
            {
                return NotFound();
            }

            return ingresso;
        }

        // PUT: api/Ingresso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngresso(Guid id, Ingresso ingresso)
        {
            if (id != ingresso.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingresso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngressoExists(id))
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

        // POST: api/Ingresso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingresso>> PostIngresso(CreateIngressoModel model, Guid idEvento)
        {
            var ingresso = new Ingresso()
            {
                Nome = model.Nome,
                Preco = model.Preco,
                Quantidade = model.Quantidade,
                IdEvento = idEvento
            };
            
            _context.Ingressos.Add(ingresso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngresso", new { id = ingresso.Id }, ingresso);
        }

        // DELETE: api/Ingresso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngresso(Guid id)
        {
            if (_context.Ingressos == null)
            {
                return NotFound();
            }
            var ingresso = await _context.Ingressos.FindAsync(id);
            if (ingresso == null)
            {
                return NotFound();
            }

            _context.Ingressos.Remove(ingresso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngressoExists(Guid id)
        {
            return (_context.Ingressos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
