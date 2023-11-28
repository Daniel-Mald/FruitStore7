using FruitStore.Areas.Jirafa.Models;
using FruitStore.Models.Entities;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Areas.Jirafa.Controllers
{
    [Area("Jirafa")]
    public class CategoriasController : Controller
    {
        public Repository<Categorias> Repository { get; }
        public CategoriasController(Repository<Categorias> repository)
        {
            Repository = repository;
        }
        public IActionResult Index()
        {
            AdminCategoriasViewModel vm = new()
            {
                Categorias = Repository.GetAll().
                OrderBy(x => x.Nombre)
                .Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                })
            };
            return View(vm);
        }
        public IActionResult Agregar()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Agregar(Categorias c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
            {
                ModelState.AddModelError("", "Agrega un nombre");
            }
            if (ModelState.IsValid)
            {
                Repository.Insert(c);
                return RedirectToAction("Index");
            }

            return View(c);
        }
        public IActionResult Editar(int id)
        {
            var cat = Repository.Get(id);
            if(cat == null)
            {
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        [HttpPost]
        public IActionResult Editar(Categorias c)
        {
            var catDB = Repository.Get(c.Id);
            if (catDB == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(c.Nombre))
                {
                    ModelState.AddModelError("", "Debe escribir el nombre de la categoria");

                }
                if (ModelState.IsValid)
                {
                    catDB.Nombre = c.Nombre;
                    Repository.Update(catDB);
                    return RedirectToAction("Index");
                }
            }
          
            return View(c);
        }





        public IActionResult Eliminar(int id)
        {
            var cat = Repository.Get(id);
            if (cat == null)
            {
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        [HttpPost]
        public IActionResult Eliminar(Categorias c)
        {
            var cat = Repository.Get(c.Id);
            if(cat != null)
            {
                Repository.Delete(cat);
            }
            return RedirectToAction("Index");
        }
    }
}
