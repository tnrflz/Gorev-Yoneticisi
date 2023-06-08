using GorevYoneticisiProjesi.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = GorevYoneticisiProjesi.Entity.Task;

namespace GorevYoneticisiProjesi.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class TaskController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TaskController(DatabaseContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet] 
        public IActionResult GetAll() {

            var values = _context.Tasks.ToList();
            return Ok(values);

        }

        [HttpPost]
        public IActionResult Add(Task task) {

            _context.Add(task);
            _context.SaveChanges();
            return Ok();

        }


        [HttpPut]

        public IActionResult Update(Task task)
        {

            var updateTask = _context.Find<Task>(task.Id);

            if (updateTask == null)
            {
                return NotFound();
            }

            else
            {
                updateTask.Description = task.Description;
                updateTask.Title = task.Title;

                _context.Update(updateTask);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
       
            var task = _context.Tasks.Find(id);

            if (task == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(task);
                _context.SaveChanges();
                return Ok();
            }
        }


    }
}
