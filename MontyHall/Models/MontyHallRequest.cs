namespace MontyHall.Models
{
    public class MontyHallRequest
    {
        public int IterationCount { get; set; }
        public int SelectedDoor {  get; set; }
        public bool KeepSelection { get; set; }
    }
}
