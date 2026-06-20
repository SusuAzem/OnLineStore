namespace OnLineStore.ViewModels
{
    public class ButtonViewModel
    {
        public int ItemId { get; set; }
        public string? Controller { get; set; }
        public string? Area { get; set; }
        public bool? B1 { get; set; } = false;
        public bool? B2 { get; set; } = false;
        public bool? B3 { get; set; }= false;
    }
}
