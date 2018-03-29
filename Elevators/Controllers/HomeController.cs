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

        public IActionResult Elevator(ElevatorModel model) //randomising locations and stats of 6 different elevators
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
                elevator.ElevatorBottomFloor = 0;
                elevator.ElevatorTopFloor = model.CurrentFloor;
                if (elevator.ElevatorDistance == 0)
                    elevator.ElevatorOpen = true;

                model.ElevatorList.Add(elevator);
            }

            return View(model);
        }

        public IActionResult CallElevator(ElevatorModel model)
        {
            List<Elevator> closestElevatorList = new List<Elevator>();
            Elevator closestElevator = new Elevator();

            closestElevatorList = model.ElevatorList.OrderBy(x => x.ElevatorDistance).ToList();

            if (closestElevatorList.FirstOrDefault().ElevatorDistance == 0) //End if an elevator is already on the correct floor
            {
                return PartialView("ElevatorPartial.cshtml", model);
            }

            for (int j = 0; j < closestElevatorList.Count; j++) //Adding distance if elevator is not going in the correct direction
            {
                if (model.CallUp != closestElevatorList[j].ElevatorGoingUp)
                {
                    closestElevatorList[j].ElevatorDistance = closestElevatorList[j].ElevatorDistance + 3;
                }
            }

            closestElevator = closestElevatorList.OrderBy(x => x.ElevatorDistance).Where(x => x.ElevatorFull.Equals(false)).FirstOrDefault(); //Sorting list again according to updated distances

            for (int i = 0; i < model.ElevatorList.Count; i++)
            {
                //Getting total distance all elevators travel, assuming they all travel at the same speed and continue in the direction they were going
                if (!model.ElevatorList[i].ElevatorBorked)
                    model.ElevatorList[i] = ElevatorTravel(model.ElevatorList[i], closestElevator.ElevatorDistance);
            }

            return PartialView("ElevatorPartial.cshtml", model);
        }

        public Elevator ElevatorTravel(Elevator elev, uint travelDistance)
        {
            for (int i = 0; i < travelDistance; i++)
            {
                if (elev.ElevatorGoingUp)
                {
                    if (elev.ElevatorTopFloor == elev.ElevatorCurrentFloor + 1) //if elevator hits the highest it can go, it changes direction
                    {
                        elev.ElevatorGoingUp = false;
                    }

                    elev.ElevatorCurrentFloor++;
                }
                if (!elev.ElevatorGoingUp)
                {
                    if (elev.ElevatorBottomFloor == elev.ElevatorCurrentFloor - 1)//if elevator hits the lowest it can go, it changes direction
                    {
                        elev.ElevatorGoingUp = true;
                    }

                    elev.ElevatorCurrentFloor--;
                }
            }

            return elev;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

