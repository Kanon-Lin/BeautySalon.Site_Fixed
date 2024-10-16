using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models
{
    public class TimeSlotResponse
    {
        public string TimeSlot { get; set; }
        public bool IsBooked { get; set; }
        public bool IsAvailable { get; set; }
        public int Duration { get; set; }
    }
}