namespace MontyHall.Models.Dto
{
    public class MontyHallRequestDto
    {
        public int IterationCount { get; set; }
        public int SelectedDoor { get; set; }
        public bool KeepSelection { get; set; }
    }
}
