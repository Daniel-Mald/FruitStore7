using FruitStore.Areas.Jirafa.Models;
using FruitStore.Models.Entities;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Areas.Jirafa.Controllers
{
    [Authorize]
    [Area("Jirafa")]
    public class ProductosController : Controller
    {
        ProductosRepository pR;
        Repository<Categorias> cR;
        public ProductosController(ProductosRepository productosRepository, Repository<Categorias> categoriasRepository)
        {
            pR = productosRepository;
            cR = categoriasRepository;
        }

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Supervisor, Administrador")]
        public IActionResult Index(AdminProductosViewModel vm)
        {
            vm.Categorias = cR.GetAll().OrderBy(x => x.Nombre).Select(x => new CategoriaModel
            {
                Id = x.Id,
                Nombre = x.Nombre ?? ""
            });
            if (vm.IdCategoriaSeleccioneada == 0)
            {
                vm.Productos = pR.GetAll().Select(x => new ProductoModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? "",
                    Categoria = x.IdCategoriaNavigation?.Nombre ?? ""
                });

            }
            else
            {
                vm.Productos = pR.GetProductosByCategoria(vm.IdCategoriaSeleccioneada).Select(x => new ProductoModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? "",
                    Categoria = x.IdCategoriaNavigation?.Nombre ?? ""
                });
            }
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            var data = new AdminAgregarProductosViewModel
            {
                Categorias = cR.GetAll().OrderBy(x=>x.Nombre).Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                })
            };
            return View(data);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar(AdminAgregarProductosViewModel vm)
        {
            //validar
            if(vm.Archivo!=null)//si selecciono un archivo
            {
                //Mime type
                if(vm.Archivo.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permiten imagenes JPG");
                }
                if(vm.Archivo.Length >500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten aarchivos no mayores a  500kb");
                }
            }


            if (ModelState.IsValid)
            {
                pR.Insert(vm.Productos); 
                //entity framework lo actualiza y ya tiene id
                if(vm.Archivo == null)//No eligio archivo
                {
                    //Obtener el Id del archivo
                    //Copiar el archivo llamado nodisponible.jpg y cambiar el nombre por el id

                    System.IO.File.Copy("wwwroot/img_frutas/0.jpg",$"wwwroot/img_frutas/{vm.Productos.Id}.jpg");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img_frutas/{vm.Productos.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            var data = new AdminAgregarProductosViewModel
            {
                Categorias = cR.GetAll().OrderBy(x => x.Nombre).Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                })
            };
            return View(data);

        }
        public IActionResult Editar(int id)
        {
            var p = pR.Get(id);
            if(p== null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarProductosViewModel vm = new()
                {
                    Productos = p,
                    Categorias = cR.GetAll().OrderBy(x => x.Nombre).Select(x => new CategoriaModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre ?? ""
                    })
                    //Archivo = 
                };
                return View(vm);
            }
            
            
        }
        [HttpPost]
        public IActionResult Editar(AdminAgregarProductosViewModel vm)
        {
            if (vm.Archivo != null)//si selecciono un archivo
            {
                //Mime type
                if (vm.Archivo.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permiten imagenes JPG");
                }
                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten aarchivos no mayores a  500kb");
                }
            }
            if (ModelState.IsValid)
            {
                var producto = pR.Get(vm.Productos.Id);
                if(producto == null)
                {
                    return RedirectToAction("Index");
                }
                producto.Nombre = vm.Productos.Nombre;
                producto.Precio = vm.Productos.Precio;
                producto.Descripcion = vm.Productos.Descripcion;
                producto.UnidadMedida = vm.Productos.UnidadMedida;
                producto.IdCategoria = vm.Productos.IdCategoria;
               

                pR.Update(producto);

                //editar la foto

                if(vm.Archivo != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img_frutas/{vm.Productos.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();

                }
                return RedirectToAction("Index");   
            }
            vm.Categorias = cR.GetAll().OrderBy(x => x.Nombre).Select(x => new CategoriaModel
            {
                Id = x.Id,
                Nombre = x.Nombre ?? ""
            });
            return View(vm);
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var producto = pR.Get(id);
            if(producto == null)
            {
                return RedirectToAction("Index");
            }
            return View(producto);
        }


        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            var producto = pR.Get(p.Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            var ruta = $"wwwroot/img_frutas/{p.Id}.jpg";
            pR.Delete(producto);
            if (System.IO.File.Exists(ruta))
            {

                System.IO.File.Delete(ruta);
            }
            return RedirectToAction("Index");
        }
    }
    
}
