namespace MontyHall.Models.Dto
{
    public class MontyHallResponseDto
    {
        public List<Iteration> Iterations { get; set; }
        public int CarWinCount { get; set; }
        public int GoatWinCount { get; set; }
    }

    public class Iteration
    {
        public List<DoorDto> InitialState { get; set; }
        public List<DoorDto> FirstDoorOpenState { get; set; }
        public List<DoorDto> SecondDoorOpenState { get; set; }
        public string Won { get; set; }
    }

    public class DoorDto
    {
        public string? Obj { get; set; }
        public string? Status { get; set; }
    }
}
