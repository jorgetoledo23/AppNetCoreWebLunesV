using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly AppDbContext _context;
        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult HomeCategorias()
        {
            List<Categoria> listaCategorias = _context.tblCategorias.ToList();
            return View(listaCategorias);
        }

        [HttpGet]
        public IActionResult EditCategoria(int CategoriaId)
        {
            var Categoria = _context.tblCategorias.Where(c => c.CategoriaId.Equals(CategoriaId)).FirstOrDefault();
            return View(Categoria);
        }

        [HttpPost]
        public IActionResult EditCategoria(Categoria C)
        {
            var CategoriaEditada = _context.tblCategorias.Where(c => c.CategoriaId.Equals(C.CategoriaId)).FirstOrDefault();
            if (CategoriaEditada != null)
            {
                CategoriaEditada.Nombre = C.Nombre;
                CategoriaEditada.Descripcion = C.Descripcion;
                if (ModelState.IsValid)
                {
                    var entrada = _context.Attach(CategoriaEditada);
                    entrada.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(HomeCategorias));
                }
                else
                {
                    return View(C);
                }
            }
            else {
                return NotFound();
            }
           
        }


        [HttpGet]
        public IActionResult CrearCategoria() {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCategoria(Categoria C)
        {
            if (ModelState.IsValid)
            {
                _context.Add(C);
                _context.SaveChanges();
                return RedirectToAction(nameof(HomeCategorias));
            }
            else
            {
                return View(C);
            }

        }

    }
}
