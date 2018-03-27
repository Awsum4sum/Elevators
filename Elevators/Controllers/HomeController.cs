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
            model.FloorCount = random.Next(5, 100);
            model.CurrentFloor = random.Next(model.FloorCount);
            model.ElevatorList = new List<Elevator>();

            for (int i = 0; i < 6; i++)
            {
                Elevator elevator = new Elevator();
                int elevatorDirection = random.Next(2);

                elevator.ElevatorCurrentFloor = random.Next(0, model.FloorCount);
                elevator.ElevatorGoingUp = Convert.ToBoolean(elevatorDirection);
                elevator.ElevatorID = i;
                elevator.ElevatorDistance = Convert.ToUInt32(Math.Abs(model.CurrentFloor - elevator.ElevatorCurrentFloor));
                elevator.ElevatorOpen = false;
                if (elevator.ElevatorDistance == 0)
                    elevator.ElevatorOpen = true;

                model.ElevatorList.Add(elevator);
            }

            return View(model);
        }

        public IActionResult CallElevator(ElevatorModel model)
        {


            return PartialView("ElevatorPartial.cshtml", model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
