namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class AddressInfo
    {
        /// <summary>
        /// Address line 1
        /// </summary>
        public string? AddressLine1 { get; set; }
        /// <summary>
        /// Address line 2
        /// </summary>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// Address line 3
        /// </summary>
        public string? AddressLine3 { get; set; }
        /// <summary>
        /// Address line 4
        /// </summary>
        public string? AddressLine4 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// Zip code + 4
        /// </summary>
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? AddressType { get; set; }
        /// <summary>
        /// Premise code
        /// </summary>
        public string? PremCode { get; set; }
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? SuffixCode { get; set; }
        public string? UnitNumber { get; set; }
        public string? PreDirection { get; set; }
        public string? PostDirection { get; set; }
        public string? UnitTypeCode { get; set; }
        public string? FullAddress { get; set; }
        public string? StartDate { get; set; }
        public string? StopDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreationDate { get; set; }
        public string? Id { get; set; }
        public string? Action { get; set; }
        public bool GarbageInactiveInd { get; set; }
        /// <summary>
        /// Premise type
        /// </summary>
        public string? PremiseType { get; set; }
        public string? ParcelId { get; set; }
        public string? Status { get; set; }
        public string? TodoId { get; set; }
        public string? TodoStatus { get; set; }
        public string? MoreRows { get; set; }
    }
}
