using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public UtilizadorController(ES2DBContext context)
        {
            _context = context;
        }

        // GET: api/Utilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadors()
        {
          if (_context.Utilizadors == null)
          {
              return NotFound();
          }
            return await _context.Utilizadors.ToListAsync();
        }

        // GET: api/Utilizador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(Guid id)
        {
            if (_context.Utilizadors == null)
            { 
                return NotFound();
            }
            
            var utilizador = await _context.Utilizadors.FindAsync(id);

            if (utilizador == null)
            {
                return NotFound();
            }

            return utilizador;
        }
        
// PUT: api/Utilizador/5
// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(Guid id, UpdateUtilizadorModel model)
        {
            if (id != id)
            {
                return BadRequest();
            }

            // Carregar o utilizador existente do banco de dados
            var utilizador = await _context.Utilizadors.FindAsync(id);

            if (utilizador == null)
            {
                return NotFound();
            }

            // Atualizar as propriedades permitidas
            utilizador.Nome = model.Nome;
            utilizador.Email = model.Email;
            utilizador.Username = model.Username;
            utilizador.Senha = model.Senha;
            utilizador.Telefone = model.Telefone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadorExists(id))
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



        // POST: api/Utilizador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizador>> PostUtilizador(CreateUtilizadorModel model)
        {
            if (await _context.Utilizadors.AnyAsync(u => u.Username == model.Username))
            {
                return Conflict("Username already exists.");
            }
            
            var tipoUtilizador = model.Organizador ? "Organizador": "Participante";

            var utilizador = new Utilizador()
            {
                Nome = model.Nome,
                Email = model.Email,
                Username = model.Username,
                Senha = model.Senha,
                Telefone = model.Telefone,
                Tipo = tipoUtilizador
            }; 
            
            _context.Utilizadors.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilizador", new { id = utilizador.Id }, utilizador);
        }

        // DELETE: api/Utilizador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizador(Guid id)
        {
            if (_context.Utilizadors == null)
            {
                return NotFound();
            }
            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizadors.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilizadorExists(Guid id)
        {
            return (_context.Utilizadors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    public class UpdateUtilizadorModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
    }
}
