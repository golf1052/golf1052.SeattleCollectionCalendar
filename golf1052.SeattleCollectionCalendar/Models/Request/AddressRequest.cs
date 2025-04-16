namespace golf1052.SeattleCollectionCalendar.Models.Request
{
    public class AddressRequest
    {
        public AddressSearchObject Address { get; set; } = null!;
    }

    public class AddressSearchObject
    {
        public string? AddressLine1 { get; set; }
        public string? PremCode { get; set; }
    }
}
