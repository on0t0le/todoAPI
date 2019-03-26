using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TasksController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SimpleTask>>> GetAll()
        {
            var tasks = await _db.todos.AsNoTracking().ToListAsync();
            return tasks;
        }

        // GET: /api/tasks/id
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleTask>> GetSpecTask(int id)
        {
            var task = await _db.todos.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: /api/tasks/
        [HttpPost]
        public async Task<ActionResult<SimpleTask>> CreateTask(SimpleTask task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            _db.todos.Add(task);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpecTask), new { task.id }, task);
        }

        // PUT: /api/tasks/id
        [HttpPut("{id}")]
        public ActionResult EditTask(int id, SimpleTask task)
        {
            if (id != task.id)
            {
                return BadRequest();
            }

            _db.Entry(task).State = EntityState.Modified;
            _db.SaveChanges();

            return NoContent();
        }

        //[FromBody]JsonPatchDocument<SimpleTask> patchTask
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch([FromRoute]int id, JsonPatchDocument<SimpleTask> patchTask)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var task = await _db.todos.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            //Apply changes to speaker object
            patchTask.ApplyTo(task);
            await _db.SaveChangesAsync();

            //update speaker in database and return updated object
            return Ok(patchTask);            
        }

        // DELETE: /api/tasks/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var delItem = await _db.todos.FindAsync(id);

            if(delItem == null)
            {
                return NotFound();
            }

            _db.todos.Remove(delItem);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
