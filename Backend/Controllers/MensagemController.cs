using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public MensagemController(ES2DBContext context)
        {
            _context = context;
        }

        [HttpGet("CheckMensagemByEvento/{idEvento}")]
        public async Task<ActionResult<bool>> CheckMensagemByEvento(Guid idEvento)
        {
            var exists = await _context.Mensagems.AnyAsync(m => m.IdEvento == idEvento);
            return exists;
        }

        [HttpGet("GetMensagensByEvento/{idEvento}")]
        public async Task<ActionResult<Mensagem[]>> GetMensagensByEvento(Guid idEvento)
        {
            var mensagens = await _context.Mensagems.Where(m => m.IdEvento == idEvento).ToArrayAsync();
            return mensagens;
        }
        
        // GET: api/Mensagem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensagem>>> GetMensagems()
        {
          if (_context.Mensagems == null)
          {
              return NotFound();
          }
            return await _context.Mensagems.ToListAsync();
        }

        // GET: api/Mensagem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensagem>> GetMensagem(Guid id)
        {
          if (_context.Mensagems == null)
          {
              return NotFound();
          }
            var mensagem = await _context.Mensagems.FindAsync(id);

            if (mensagem == null)
            {
                return NotFound();
            }

            return mensagem;
        }

        // PUT: api/Mensagem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensagem(Guid id, Mensagem mensagem)
        {
            if (id != mensagem.Id)
            {
                return BadRequest();
            }

            _context.Entry(mensagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensagemExists(id))
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

        // POST: api/Mensagem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mensagem>> PostMensagem(CreateMensagemModel model, Guid IdEvento)
        {
            var mensagem = new Mensagem()
            {
                Mensagem1 = model.Mensagem,
                IdEvento = IdEvento
            };
            
            _context.Mensagems.Add(mensagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensagem", new { id = mensagem.Id }, mensagem);
        }

        // DELETE: api/Mensagem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensagem(Guid id)
        {
            if (_context.Mensagems == null)
            {
                return NotFound();
            }
            var mensagem = await _context.Mensagems.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }

            _context.Mensagems.Remove(mensagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensagemExists(Guid id)
        {
            return (_context.Mensagems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
