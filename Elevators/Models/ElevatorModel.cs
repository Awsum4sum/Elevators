using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevators.Models
{
    public class ElevatorModel
    {
        public int ElevatorCount { get; set; }
        public int FloorCount { get; set; }
        public int CurrentFloor { get; set; }
        public bool CallUp { get; set; }
        public List<Elevator> ElevatorList { get; set; }
    }

    public class Elevator
    {
        public int ElevatorID { get; set; }
        public int ElevatorCurrentFloor { get; set; }
        public bool ElevatorGoingUp { get; set; }
        public bool ElevatorOpen { get; set; }
        public uint ElevatorDistance { get; set; }
    }
}
