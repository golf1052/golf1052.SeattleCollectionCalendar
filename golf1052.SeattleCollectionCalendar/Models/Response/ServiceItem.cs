using System;
using System.Collections.Generic;
using System.Text;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class ServiceItem
    {
        public string? ServicePointId { get; set; }
        public string? LastPickupDate { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? PremiseType { get; set; }
        public string? SubType { get; set; }
        public List<BinInfo>? Bins { get; set; }
        public Schedule? Schedule { get; set; }
    }

    public class BinInfo
    {
        public string? Type { get; set; }
        public string? ItemType { get; set; }
        public string? Size { get; set; }
        public string? Count { get; set; }
        public string? BinClass { get; set; }
    }

    public class Schedule
    {
        public string? Frequency { get; set; }
        public string? Mon { get; set; }
        public string? Tue { get; set; }
        public string? Wed { get; set; }
        public string? Thu { get; set; }
        public string? Fri { get; set; }
        public string? Sat { get; set; }
        public string? Sun { get; set; }
    }
}
