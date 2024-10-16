using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.ViewModels
{
    public class TimeSlotVm
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Time { get; set; }
        public int Duration { get; set; }
        public bool IsBooked { get; set; }
        public bool IsAvailable { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}