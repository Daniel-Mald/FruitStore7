using FruitStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FruitStore.Repositories
{
    public class ProductosRepository : Repository<Productos>
    {
        public ProductosRepository(FruteriashopContext context) : base(context) //para usar el contexto del padre
        {
        }
        public IEnumerable<Productos> GetProductosByCategoria( string categoria)
        {
            return Context.Productos
                .Include(x=>x.IdCategoriaNavigation)
                .Where(x =>x.IdCategoriaNavigation!=null && x.IdCategoriaNavigation.Nombre == categoria)
                
                .OrderBy(x => x.Nombre);
        }
        public IEnumerable<Productos> GetProductosByCategoria( int categoria)
        {
            return Context.Productos
                .Include(x=>x.IdCategoriaNavigation)
                .Where(x =>x.IdCategoria == categoria)
                
                .OrderBy(x => x.Nombre);
        }
        public Productos? GetByNombre(string nombre)
        {
            return Context.Productos
                .Include(x=>x.IdCategoriaNavigation)
                .FirstOrDefault(x=>x.Nombre == nombre);
        }
        public override IEnumerable<Productos> GetAll()
        {
            return Context.Productos
                .Include(x => x.IdCategoriaNavigation)
                .OrderBy(x => x.Nombre);
        }
    }
}
