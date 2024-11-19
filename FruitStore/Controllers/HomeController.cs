using FruitStore.Helpers;
using FruitStore.Models.Entities;
using FruitStore.Models.ViewModels;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FruitStore.Controllers
{
    //Agregacion
    public class HomeController : Controller
    {
        public ProductosRepository Repository { get; }
        public Repository<Usuarios> _usuarioRepos { get; }
        public HomeController(ProductosRepository repository , Repository<Usuarios> repos)
        {
            Repository = repository;
            _usuarioRepos = repos;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Productos(string Id)//nombre de la categoria
        {
            Id = Id.Replace("-", " ");
            ProductosViewModel vm = new()
            {
                Categoria = Id,
                Productos = Repository.GetProductosByCategoria(Id)

                .Where(x => x.IdCategoriaNavigation?.Nombre == Id)
                .OrderBy(x => x.Nombre)
                .Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? "",
                    Precio = x.Precio ?? 0m,
                    FechaModificacion = new FileInfo($"wwwroot/img_frutas/{x.Id}.jpg").LastWriteTime.ToString("yyyyMMddhhmm")
                })
            };

            

            return View(vm);
        }

        public IActionResult Ver(string Id)
        {
            Id = Id.Replace("-", " ");
            var producto = Repository.GetByNombre(Id);
            if(producto == null)
            {
                return RedirectToAction("Index");
            }
            ProductoViewModel vm = new()
            {
                Id = producto.Id,
                Categoria = producto.IdCategoriaNavigation?.Nombre ?? "",
                Descripcion = producto.Descripcion ?? "",
                Precio = producto.Precio ?? 0,
                unidadDeMedida = producto.UnidadMedida ?? "",
                Nombre = producto.Nombre ?? ""
            };
            return View(vm);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login (LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
                ModelState.AddModelError("", "Escriba el correo");
            if (string.IsNullOrWhiteSpace(vm.Contraseña))
                ModelState.AddModelError("", "Escriba la contrasena");


            if (ModelState.IsValid)
            {
                var user = _usuarioRepos.GetAll().FirstOrDefault(x => x.CorreoElectronico == vm.Correo &&
                x.Contrasena == Encriptacion.StringToSHA512(vm.Contraseña));

                if(user == null)
                {
                    ModelState.AddModelError("", "Contrasena o correo incorrectos");
                }
                else
                {
                    //Loguear
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol==1?"Administrador":"Supervisor"));

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true
                    }) ;
                    return RedirectToAction("Index", "Home", new {area = "Jirafa"});   
                }
            }
            return View(vm);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home" );
        }
        public IActionResult Denied()
        {
            return View();
        }

    }
}
