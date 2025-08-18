using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

public class PropietarioController : Controller
{
    public IActionResult Index()
{
    var lista = new List<Propietario>
    {
        new Propietario { Id_propietarios = 1,Apellido="jose", Nombre = "Juan", Telefono = "26654645665",Email = "juan@juan",Dni="2317822",Estado=true},
        
    };


    return View(lista);
    }

    public IActionResult Detalle(int id)
    {
        // Ejemplo: mostrar un propietario específico
        return View();
    }
}