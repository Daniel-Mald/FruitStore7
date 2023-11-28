using System.Reflection.Metadata.Ecma335;

namespace FruitStore.Models.ViewModels
{
    public class ProductoViewModel
    {
        public string Nombre { get; set; } = null!;
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = null!;
        public string unidadDeMedida { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}
