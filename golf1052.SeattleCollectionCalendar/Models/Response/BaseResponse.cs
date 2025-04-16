namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public abstract class BaseResponse
    {
        public string Status { get; set; } = null!;
        public string StatusCode { get; set; } = null!;
        public string RequestTransactionId { get; set; } = null!;
    }
}
