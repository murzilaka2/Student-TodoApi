using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IUser _users;

        public TodoController(ApplicationContext context, IUser users)
        {
            _context = context;
            _users = users;
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoDTO)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _users.GetUserByEmailAsync(userEmail);
            if (currentUser != null)
            {
                var todoItem = new TodoItem
                {
                    IsComplete = todoDTO.IsComplete,
                    Name = todoDTO.Name,
                    UserId = currentUser.Id
                };

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTodoItem),
                    new { id = todoItem.Id },
                    ItemToDTO(todoItem));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return ItemToDTO(todoItem);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItem()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _users.GetUserByEmailAsync(userEmail);
            if (currentUser != null)
            {
                return await _context.TodoItems.Where(e => e.UserId == currentUser.Id)
                            .Select(x => ItemToDTO(x))
                            .ToListAsync();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return BadRequest();
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _users.GetUserByEmailAsync(userEmail);
            if (currentUser != null)
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }
                if (todoItem.UserId != currentUser.Id)
                {
                    return NotFound();
                }

                todoItem.Name = todoDTO.Name;
                todoItem.IsComplete = todoDTO.IsComplete;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _users.GetUserByEmailAsync(userEmail);
            if (currentUser != null)
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }

                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
      new TodoItemDTO
      {
          Id = todoItem.Id,
          Name = todoItem.Name,
          IsComplete = todoItem.IsComplete
      };

    }
}
