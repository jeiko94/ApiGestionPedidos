using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestionPedidos.Dominio.Models
{
    //Representa un producto disponible en el sistema
    public class Producto
    {
        //Identificador unico del producto
        public int Id { get; set; }

        //Nombre del producto
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? Nombre { get; set; }

        //Precio unitario del producto
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        //Strock disponible del producto
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }
    }
}
