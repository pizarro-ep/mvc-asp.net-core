using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserItemsController : ControllerBase
    {
        private readonly ApiContext _context;

        public UserItemsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/UserItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItemDTO>>> GetUserItems()
        {
            // return await _context.UserItems.ToListAsync();
            return await _context.UserItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/UserItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItemDTO>> GetUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(userItem);        }

        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(long id, UserItemDTO userItemDTO)
        {
            if (id != userItemDTO.Id)
            {
                return BadRequest();
            }

            var userItem = await _context.UserItems.FindAsync(id);
            if(userItem == null ){
                return NotFound();
            }

            userItem.Username = userItemDTO.Username;
            userItem.IsActive = userItemDTO.IsActive;
            //_context.Entry(userItemDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
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

        // POST: api/UserItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItemDTO>> PostUserItem(UserItemDTO userItemDTO)
        {
            var userItem = new UserItem{
                IsActive = userItemDTO.IsActive,
                Username = userItemDTO.Username
            };

            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserItem)/*"GetUserItem"*/, new { id = userItemDTO.Id }, ItemToDTO(userItem));
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(userItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserItemExists(long id)
        {
            return _context.UserItems.Any(e => e.Id == id);
        }
        private static UserItemDTO ItemToDTO(UserItem userItem) => new UserItemDTO{
            Id = userItem.Id,
            Username = userItem.Username,
            IsActive = userItem.IsActive
        };
    }
}
