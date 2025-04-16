using System;
using System.Collections.Generic;
using System.Linq;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class SolidWasteServices
    {
        public string? ServiceId { get; set; }
        public string? PremiseId { get; set; }
        public string? PremiseType { get; set; }
        public string? Condition { get; set; }
        public int BinCount { get; set; }
        public List<ServiceItem>? Services { get; set; }

        public List<ServiceItem>? GetServiceItem(SolidWasteType solidWasteType)
        {
            if (Services == null)
            {
                return null;
            }

            return Services.Where(s => solidWasteType switch
                {
                    SolidWasteType.Garbage => "Garbage".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    SolidWasteType.Recycle => "Recycle".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    SolidWasteType.FoodYardWaste => "Food/Yard Waste".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    _ => false
                })
                .ToList();
        }
    }
}
