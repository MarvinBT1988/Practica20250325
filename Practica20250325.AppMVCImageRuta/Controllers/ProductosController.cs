using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica20250325.AppMVCImageRuta.Models;

namespace Practica20250325.AppMVCImageRuta.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Practica20250325IrutaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductosController(Practica20250325IrutaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment= webHostEnvironment;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        public async Task<string> GuardarImage(IFormFile? file, string url = "")
        {
            string urlImage = url;
            if (file != null && file.Length > 0)
            {
                // Construir la ruta del archivo
                string nameFile = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes", nameFile);

                // Guardar la imagen en wwwroot
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Guardar la ruta en la base de datos
                urlImage = "/imagenes/" + nameFile;
            }
            return urlImage;
        }
        public async Task<bool> EliminarImage(string urlImage)
        {
            if (string.IsNullOrEmpty(urlImage))
            {
                return false;
            }

            try
            {
                // Construir la ruta completa del archivo
                string nameFile = Path.GetFileName(urlImage);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes", nameFile);

                // Verificar si el archivo existe y eliminarlo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                else
                {
                    //El archivo no existe.
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante la eliminación
                Console.WriteLine($"Error al eliminar la imagen: {ex.Message}");
                return false;
            }
        }
        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Descripcion,Precio,ImagenRuta")] Producto producto,IFormFile? file=null)
        {
            if (ModelState.IsValid)
            {
                producto.ImagenRuta = await GuardarImage(file);
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,ImagenRuta")] Producto producto, IFormFile? file = null)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var rutaImagenAnterior = await _context.Productos
                        .Where(s => s.ProductoId == producto.ProductoId)
                        .Select(s => s.ImagenRuta).FirstOrDefaultAsync();

                    producto.ImagenRuta = await GuardarImage(file,producto.ImagenRuta);
                    _context.Update(producto);
                    int result=await _context.SaveChangesAsync();
                    if (result > 0 && rutaImagenAnterior!=null && rutaImagenAnterior.Length>0)
                    {
                        if(rutaImagenAnterior != producto.ImagenRuta)
                        {
                            await EliminarImage(rutaImagenAnterior);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
              
                _context.Productos.Remove(producto);
            }
            var rutaImagen = producto.ImagenRuta;
            int result= await _context.SaveChangesAsync();
            if (result > 0 && !string.IsNullOrWhiteSpace(rutaImagen))
            {
                await EliminarImage(rutaImagen);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
    }
}
