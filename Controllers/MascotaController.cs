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
        public IActionResult Registrar(MascotaViewModel viewModel)
        {
            var fechaNacimiento = viewModel.FormMascota.FechaNacimiento.ToUniversalTime();
            var mascota = new Mascota
            {
                Nombre = viewModel.FormMascota.Nombre,
                Raza = viewModel.FormMascota.Raza,
                Color = viewModel.FormMascota.Color,
                FechaNacimiento = fechaNacimiento
            };

            _context.DBMasc.Add(mascota);
            _context.SaveChanges();

            ViewData["Message"] = "Mascota Registrada";

            var misMascotas = _context.DBMasc.ToList();
            viewModel.ListMascota = misMascotas;

            return View("Index", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}