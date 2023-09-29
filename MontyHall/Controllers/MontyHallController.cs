using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontyHall.Models;
using MontyHall.Models.Dto;
using MontyHall.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace MontyHall.Controllers
{
    [Route("api/run_simulation")]
    [ApiController]
    public class MontyHallController : ControllerBase
    {

        [HttpPost]
        public ActionResult<MontyHallResponseDto> RunSimulation([FromBody] MontyHallRequestDto requestDto)
        {

            int iterationCount = requestDto.IterationCount;
            bool keepSelection = requestDto.KeepSelection;
            int selectedDoor = requestDto.SelectedDoor;

            int carWinCount = 0;
            int goatWinCount = 0;

            List<Iteration> iterationList = new List<Iteration>();

            for (int i = 0; i< iterationCount; i++)
            {
                Iteration iteration = new Iteration();

                List<DoorDto> currentDoors = InitSimulation();
                iteration.InitialState = copyCurrentDoorsState(currentDoors);

                int firstOpenedDoorIndex = openFirstDoor(currentDoors, selectedDoor);
                openDoorByIndex(currentDoors, firstOpenedDoorIndex);
                iteration.FirstDoorOpenState = copyCurrentDoorsState(currentDoors);

                int secondOpenedDoorIndex = openSecondDoor(currentDoors, selectedDoor, keepSelection);
                openDoorByIndex(currentDoors, secondOpenedDoorIndex);
                iteration.SecondDoorOpenState = copyCurrentDoorsState(currentDoors);

                if (isWinCar(currentDoors))
                {
                    iteration.Won = "Car";
                    carWinCount++;
                }
                else
                {
                    iteration.Won = "Goat";
                    goatWinCount++;
                }

                iterationList.Add(iteration);
            }

            Console.WriteLine("goat " + goatWinCount);
            Console.WriteLine("car " + carWinCount);
         
            var responseDto = new MontyHallResponseDto
            {
                Iterations = iterationList,
                CarWinCount = carWinCount,
                GoatWinCount = goatWinCount,
            };

            return Ok(responseDto);
        }

        private List<DoorDto> copyCurrentDoorsState( List<DoorDto> currentDoors)
        {
            return currentDoors.ConvertAll(door => new DoorDto() { Obj = door.Obj, Status = door.Status });
        }

        private List<DoorDto> InitSimulation () {
            // Create instances of DoorDto
            DoorDto door1 = new DoorDto { Obj = "Goat", Status = "Closed" };
            DoorDto door2 = new DoorDto { Obj = "Goat", Status = "Closed" };
            DoorDto door3 = new DoorDto { Obj = "Car", Status = "Closed" };

            List<DoorDto> doors = new List<DoorDto>();
            doors.Add(door1);
            doors.Add(door2);
            doors.Add(door3);

            GenUtility.Shuffle(doors);

            return doors;
        }

        private int openFirstDoor(List<DoorDto> currentDoors, int userSelection)
        {
            DoorDto openingDoor = currentDoors.Where((item, index) => index != userSelection).Where((item, index) => item.Obj != "Car").First();
            return currentDoors.IndexOf(openingDoor);
        }

        private int openSecondDoor(List<DoorDto> currentDoors, int userSelection, bool keepSelection)
        {
            if (keepSelection)
            {
                return userSelection;
            }

            DoorDto openingDoor = currentDoors.Where((item, index) => index != userSelection && item.Status == "Closed").First();
            return currentDoors.IndexOf(openingDoor);
        }

        private bool isWinCar(List<DoorDto> currentDoors)
        {
            return currentDoors.Where((item, index) => item.Obj == "Car" && item.Status == "Open").ToArray().Length > 0;
        }

        private void openDoorByIndex(List<DoorDto> currentDoors, int doorIndex)
        {
            currentDoors[doorIndex].Status = "Open";
        }
    }
}
