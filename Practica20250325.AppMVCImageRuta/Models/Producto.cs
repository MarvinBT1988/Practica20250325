using System;
using System.Collections.Generic;

namespace Practica20250325.AppMVCImageRuta.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public string? ImagenRuta { get; set; }
}
