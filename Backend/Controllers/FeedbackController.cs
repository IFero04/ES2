using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ES2DBContext _context;

        public FeedbackController(ES2DBContext context)
        {
            _context = context;
        }
        
        // GET: api/Feedback/ByIngresso/{idIngresso}
        [HttpGet("ByInscricao/{idInscricao}")]
        public async Task<ActionResult<Feedback>> GetFeedbackByInscricao(Guid idInscricao)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.IdInscricao == idInscricao);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }
        
        [HttpGet("CheckFeedbackByInscricao/{idInscricao}")]
        public async Task<ActionResult<bool>> CheckFeedbackByInscricao(Guid idInscricao)
        {
            var haveFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.IdInscricao == idInscricao);

            return haveFeedback != null;
        }

        //CRUD -- INICIO --
        
        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(CreateFeedbackModel model)
        {
            var feedback = new Feedback()
            {
                Comentario = model.Comentario,
                IdInscricao = model.IdInscricao
            };
            
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }
        
        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }
        
        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }
        
        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(Guid id, Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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
        
        

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //CRUD -- FIM --
        
        private bool FeedbackExists(Guid id)
        {
            return (_context.Feedbacks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
