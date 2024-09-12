using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Models;

namespace CRUD.ViewModel
{
    public class MascotaViewModel
    {
        public Mascota FormMascota { get; set; }
        public IEnumerable<Mascota> ListMascota { get; set; }
    }
}