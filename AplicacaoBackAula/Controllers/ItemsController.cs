using AplicacaoBackAula.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoBackAula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly BdAulaBackendContext _context;

        public ItemsController(BdAulaBackendContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }


        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var Item = await _context.Items
                .FirstOrDefaultAsync(c => c.Iditem == id);

            if (Item == null)
            {
                return NotFound();
            }

            return Item;
        }


        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item Item)
        {
            _context.Items.Add(Item);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetItem),
                new { id = Item.Iditem },
                Item
            );
        }


        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(
            int id,
            Item Item)
        {
            if (id != Item.Iditem)
            {
                return BadRequest();
            }

            _context.Entry(Item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExiste(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }


        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var Item = await _context.Items
                .FirstOrDefaultAsync(c => c.Iditem == id);

            if (Item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(Item);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ItemExiste(int id)
        {
            return _context.Items
                .Any(e => e.Iditem == id);
        }
    }
}