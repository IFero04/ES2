using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public EventoController(ES2DBContext context)
        {
            _context = context;
        }

        // GET: api/Evento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
          if (_context.Eventos == null)
          {
              return NotFound();
          }
            return await _context.Eventos.ToListAsync();
        }

        // GET: api/Evento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(Guid id)
        {
          if (_context.Eventos == null)
          {
              return NotFound();
          }
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        // PUT: api/Evento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(Guid id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        // POST: api/Evento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento([FromBody]CreateEventModel model)
        {
            var evento = new Evento()
            {
                Nome = model.Nome,
                Data = model.Data,
                Hora = model.Hora,
                Local = model.Local,
                Descricao = model.Descricao,
                Categoria = model.Categoria,
                Capacidade = model.Capacidade,
            };

            var cont = 0;
            
            if (model.Ingressos != null)
            {
                foreach (var ingresso in model.Ingressos)
                {
                    if (ingresso.Quantidade + cont > model.Capacidade)
                    {
                        break;
                    }
                    cont += ingresso.Quantidade;
                    evento.Ingressos.Add(new Ingresso()
                    {
                        Nome = ingresso.Nome,
                        Preco = ingresso.Preco,
                        Quantidade = ingresso.Quantidade,
                        IdEvento = evento.Id
                    });
                }
            }
            
            if (model.Atividades != null)
            {
                foreach (var atividade in model.Atividades)
                {
                    evento.Atividades.Add(new Atividade()
                    {
                        Nome = atividade.Nome,
                        Data = atividade.Data,
                        Hora = atividade.Hora,
                        Descricao = atividade.Descricao,
                        IdEvento = evento.Id
                    });
                }
            }
            
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = evento.Id }, evento);
        }

        // DELETE: api/Evento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(Guid id)
        {
            if (_context.Eventos == null)
            {
                return NotFound();
            }
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(Guid id)
        {
            return (_context.Eventos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
