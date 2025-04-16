using System.Collections.Generic;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    /// <summary>
    /// Address search response. If the address was not found, Address will be empty. 
    /// </summary>
    public class AddressSearchResponse : BaseResponse
    {
        public List<AddressInfo> Address { get; set; } = new List<AddressInfo>();
    }
}
