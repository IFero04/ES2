using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;
using Frontend.Pages.Organizador;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtividadeController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public AtividadeController(ES2DBContext context)
        {
            _context = context;
        }
        
        [HttpGet("CheckAtividadesByEvento/{idEvento}")]
        public async Task<ActionResult<bool>> CheckAtividadesByEvento(Guid idEvento)
        {
            var haveAtividade = await _context.Atividades.FirstOrDefaultAsync(a => a.IdEvento == idEvento);

            return haveAtividade != null;
        }

        // GET: api/Atividade/GetAtividadesByEvento/{idEvento}
        [HttpGet("GetAtividadesByEvento/{idEvento}")]
        public async Task<ActionResult<IEnumerable<Atividade>>> GetAtividadesByEvento(Guid idEvento)
        {
            var atividades = await _context.Atividades
                .Where(a => a.IdEvento == idEvento)
                .ToListAsync();

            if (atividades.Count == 0)
            {
                return NotFound();
            }

            return atividades;
        }

        
        // GET: api/Atividade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Atividade>>> GetAtividades()
        {
          if (_context.Atividades == null)
          {
              return NotFound();
          }
            return await _context.Atividades.ToListAsync();
        }

        // GET: api/Atividade/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Atividade>> GetAtividade(Guid id)
        {
          if (_context.Atividades == null)
          {
              return NotFound();
          }
            var atividade = await _context.Atividades.FindAsync(id);

            if (atividade == null)
            {
                return NotFound();
            }

            return atividade;
        }
        
        //PUT
        [HttpPut("{idAtividade}")]
        public async Task<IActionResult> PutAtividade(Guid idAtividade, Atividade atividadeUpdate)
        {
            if (idAtividade != atividadeUpdate.Id)
            {
                return BadRequest();
            }

            _context.Entry(atividadeUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtividadeExists(idAtividade))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            
            var atividade = await _context.Atividades.FindAsync(idAtividade);

            if (atividade == null)
            {
                return NotFound();
            }

            atividade.Nome = atividadeUpdate.Nome;
            atividade.Data = atividadeUpdate.Data;
            atividade.Hora = atividadeUpdate.Hora;
            atividade.Descricao = atividadeUpdate.Descricao;
            atividade.IdEventoNavigation = atividade.IdEventoNavigation;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtividadeExists(idAtividade))
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

        // POST: api/Atividade
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Atividade>> PostAtividade(CreateAtividadeModel model)
        {
            var atividade = new Atividade()
            {
                Nome = model.Nome,
                Data = model.Data,
                Hora = model.Hora,
                Descricao = model.Descricao,
                IdEvento = model.IdEvento
            };

            _context.Atividades.Add(atividade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAtividade", new { id = atividade.Id }, atividade);
        }

        // DELETE: api/Atividade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtividade(Guid id)
        {
            var atividade = await _context.Atividades.FindAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }

            _context.Atividades.Remove(atividade);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        

        private bool AtividadeExists(Guid id)
        {
            return (_context.Atividades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
