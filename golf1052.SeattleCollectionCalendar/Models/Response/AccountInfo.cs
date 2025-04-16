using System;
using System.Collections.Generic;
using System.Text;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class AccountInfo
    {
        /// <summary>
        /// Account number
        /// </summary>
        public string? AccountNumber { get; set; }
        /// <summary>
        /// Service address
        /// </summary>
        public string? ServiceAddress { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AccountStatus { get; set; }
        /// <summary>
        /// Person Id
        /// </summary>
        public string? PersonId { get; set; }
        /// <summary>
        /// Company code, should always be SPU
        /// </summary>
        public string? CompanyCd { get; set; }
        public string? PickupDay { get; set; }
        public string? PickupDate { get; set; }
        public string? PickupMonth { get; set; }
        public List<string>? PendingAlerts { get; set; }
        public int GarbageCount { get; set; }
        public int FoodCount { get; set; }
        public int DumpsterCount { get; set; }
        public int RecycleCount { get; set; }
        public string? PersonType { get; set; }
        public List<SolidWasteServices>? SwServices { get; set; }
    }
}
