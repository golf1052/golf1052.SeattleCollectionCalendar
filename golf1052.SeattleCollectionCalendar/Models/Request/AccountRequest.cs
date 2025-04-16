using System.Collections.Generic;

namespace golf1052.SeattleCollectionCalendar.Models.Request
{
    public class AccountRequest
    {
        public AccountContext AccountContext { get; set; } = null!;
        public string? CustomerId { get; set; }
        public List<string>? ServicePoints { get; set; }
    }

    public class AccountContext
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string? CompanyCd { get; set; }
        public string? PersonId { get; set; }
    }
}
