using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUD.Data;
using CRUD.Models;
using CRUD.ViewModel;

namespace CRUD.Controllers
{
  
     public class MascotaController : Controller
    {
       private readonly ILogger<MascotaController> _logger;
        private readonly ApplicationDbContext _context;

        public MascotaController(ILogger<MascotaController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var misMascotas = _context.DBMasc.ToList();
            _logger.LogDebug("Mascotas: {misMascotas}", misMascotas);
            var viewModel = new MascotaViewModel
            {
                FormMascota = new Mascota(),
                ListMascota = misMascotas
            };
            _logger.LogDebug("ViewModel: {viewModel}", viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Insertar(MascotaViewModel viewModel)
        {
            var fechaNacimiento = viewModel.FormMascota.FechaNacimiento.ToUniversalTime();
            if (viewModel.FormMascota.Id == 0)
            {
                var mascota = new Mascota
                {
                    Nombre = viewModel.FormMascota.Nombre,
                    Raza = viewModel.FormMascota.Raza,
                    Color = viewModel.FormMascota.Color,
                    FechaNacimiento = fechaNacimiento
                };
                _context.DBMasc.Add(mascota);
                ViewData["Message"] = "Mascota Insertada con éxito";
            }
            else
            {
                var mascotaExistente = _context.DBMasc.Find(viewModel.FormMascota.Id);
                if (mascotaExistente != null)
                {
                    mascotaExistente.Nombre = viewModel.FormMascota.Nombre;
                    mascotaExistente.Raza = viewModel.FormMascota.Raza;
                    mascotaExistente.Color = viewModel.FormMascota.Color;
                    mascotaExistente.FechaNacimiento = fechaNacimiento;
                    ViewData["Message"] = "Mascota Actualizada con éxito";
                }
            }
            _context.SaveChanges();
            viewModel.ListMascota = _context.DBMasc.ToList();

            return View("Index", viewModel);
        }

        public IActionResult Eliminar(long id)
        {
            var mascota = _context.DBMasc.Find(id);
            _context.DBMasc.Remove(mascota);
            _context.SaveChanges();

            ViewData["Message"] = "Mascota Eliminada con éxito";

            var misMascotas = _context.DBMasc.ToList();
            var viewModel = new MascotaViewModel
            {
                FormMascota = new Mascota(),
                ListMascota = misMascotas
            };

            return View("Index", viewModel);
        }

        public IActionResult Editar(long id)
        {
            var mascota = _context.DBMasc.Find(id);
            var viewModel = new MascotaViewModel
            {
                FormMascota = mascota,
                ListMascota = _context.DBMasc.ToList()
            };

            return View("Index", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}