using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Elevators.Models;

namespace Elevators.Controllers
{
    public class HomeController : Controller
    {
        Random random = new Random();

        public IActionResult Index()
        {
            return RedirectToAction("Elevator");
        }

        public IActionResult Elevator(ElevatorModel model)
        {
            model.ElevatorCount = 1;
            model.FloorCount = random.Next(1, 100);
            model.CurrentFloor = random.Next(model.FloorCount);
            model.ElevatorList = new List<Elevator>();

            for(int i = 0; i < 5; i++)
            {
                Elevator elevator = new Elevator();
                int elevatorDirection = random.Next(2);

                elevator.ElevatorCurrentFloor = random.Next(0, model.FloorCount);
                elevator.ElevatorGoingUp = Convert.ToBoolean(elevatorDirection);
                elevator.ElevatorID = i;

                model.ElevatorList.Add(elevator);
            }

            return View(model);
        }

        public IActionResult ElevatorPartial(ElevatorModel model)
        {
            return PartialView("ElevatorPartial.cshtml", model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
