using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdiManager.Data;
using PdiManager.Models;

namespace PdiManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public TaskController(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        // GET
        public IActionResult Add(int pdiId)
        {
            var task = new Task();
            task.Pdi = new Pdi();
            task.Pdi.Id = pdiId;
            return View(task);
        }
        [HttpPost]
        public IActionResult Store(Task task)
        {
            if (task.Id == 0)
            {
                task.Pdi = _db.Pdis.First(registro => registro.Id == task.Pdi.Id);
                _db.Tasks.Add(task);
            }
            else
            {
                Task Task = _db.Tasks.First(registro => registro.Id == task.Id);
                Task.Name = task.Name;
            }
            _db.SaveChanges();
            return RedirectToAction("Edit","Pdi", new {id = task.Pdi.Id});
        }

        public IActionResult Done(int id)
        {
            var Task = _db.Tasks.Include("Pdi").First(t => t.Id == id);
            Task.IsDone = true;
            _db.SaveChanges();
            return RedirectToAction("Edit","Pdi", new {id = Task.Pdi.Id});
        }

        public IActionResult UnDone(int id)
        {
            var Task = _db.Tasks.Include("Pdi").First(t => t.Id == id);
            Task.IsDone = false;
            _db.SaveChanges();
            return RedirectToAction("Edit","Pdi", new {id = Task.Pdi.Id});
        }

        public IActionResult Delete(int id)
        {
            var Task = _db.Tasks.Include("Pdi").First(t => t.Id == id);
            _db.Tasks.Remove(Task);
            _db.SaveChanges();
            return RedirectToAction("Edit","Pdi", new {id = Task.Pdi.Id});
        }
    }
}