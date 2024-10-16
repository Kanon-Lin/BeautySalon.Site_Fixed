using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
    public class ActiveRegisterDto
    {
        public int memberId { get; set; }
        public string confirmCode { get; set; }
    }
}