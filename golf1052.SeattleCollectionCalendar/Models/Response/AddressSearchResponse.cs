using System.Collections.Generic;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class AddressSearchResponse : BaseResponse
    {
        public List<AddressInfo> Address { get; set; } = new List<AddressInfo>();
    }
}
