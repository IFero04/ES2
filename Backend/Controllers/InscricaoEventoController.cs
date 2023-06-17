using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricaoEventoController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public InscricaoEventoController(ES2DBContext context)
        {
            _context = context;
        }

        [HttpGet("GetEventosInscritos/{idParticipante}")]
        public async Task<ActionResult<GetInscricaoEvento>> GetEventosInscritos(Guid idParticipante)
        {
            var inscricoes = await _context.InscricaoEventos
                .Include(i => i.IdEventoNavigation)
                .Include(i => i.TipoIngressoNavigation)
                .Where(i => i.IdParticipante == idParticipante)
                .ToListAsync();

            var idEventos = inscricoes.Select(i => i.IdEvento).ToList();
            var ingressos = inscricoes.Select(i => new IngressoDetalheModel
            {
                Id = i.TipoIngressoNavigation.Id,
                Nome = i.TipoIngressoNavigation.Nome,
                Preco = i.TipoIngressoNavigation.Preco
            }).ToList();

            var model = new GetInscricaoEvento()
            {
                IdEventos = idEventos,
                Ingressos = ingressos
            };

            return model;
        }

        // GET: api/InscricaoEvento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscricaoEvento>>> GetInscricaoEventos()
        {
            var inscricaoEventos = await _context.InscricaoEventos.ToListAsync();
            return inscricaoEventos;
        }

        // GET: api/InscricaoEvento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InscricaoEvento>> GetInscricaoEvento(Guid id)
        {
            var inscricaoEvento = await _context.InscricaoEventos.FindAsync(id);

            if (inscricaoEvento == null)
            {
                return NotFound();
            }

            return inscricaoEvento;
        }

        // GET: api/InscricaoEvento/GetByEventoParticipante/{idEvento}/{idParticipante}
        [HttpGet("GetByEventoParticipante/{idEvento}/{idParticipante}")]
        public async Task<ActionResult<InscricaoEvento>> GetByEventoParticipante(Guid idEvento, Guid idParticipante)
        {
            var inscricaoEvento = await _context.InscricaoEventos.FirstOrDefaultAsync(i => i.IdEvento == idEvento && i.IdParticipante == idParticipante);

            if (inscricaoEvento == null)
            {
                return NotFound();
            }

            return inscricaoEvento;
        }

        // PUT: api/InscricaoEvento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricaoEvento(Guid id, InscricaoEvento inscricaoEvento)
        {
            if (id != inscricaoEvento.Id)
            {
                return BadRequest();
            }

            _context.Entry(inscricaoEvento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoEventoExists(id))
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

        // POST: api/InscricaoEvento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InscricaoEvento>> PostInscricaoEvento(CreateInscricaoEventoModel model)
        {
            var inscricaoEvento = new InscricaoEvento()
            {
                IdEvento = model.IdEvento,
                IdParticipante = model.IdParticipante,
                TipoIngresso = model.TipoIngresso
            };

            _context.InscricaoEventos.Add(inscricaoEvento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscricaoEvento", new { id = inscricaoEvento.Id }, inscricaoEvento);
        }

        // DELETE: api/InscricaoEvento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscricaoEvento(Guid id)
        {
            var inscricaoEvento = await _context.InscricaoEventos.FindAsync(id);
            if (inscricaoEvento == null)
            {
                return NotFound();
            }

            _context.InscricaoEventos.Remove(inscricaoEvento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("CheckInscricao/{idEvento}/{idParticipante}")]
        public async Task<ActionResult<bool>> CheckInscricao(Guid idEvento, Guid idParticipante)
        {
            var inscricaoEvento = await _context.InscricaoEventos.FirstOrDefaultAsync(i => i.IdEvento == idEvento && i.IdParticipante == idParticipante);

            return inscricaoEvento != null;
        }
        
        private bool InscricaoEventoExists(Guid id)
        {
            return _context.InscricaoEventos.Any(e => e.Id == id);
        }
    }
}
