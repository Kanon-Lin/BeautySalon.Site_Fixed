using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static Result Success()
        {
            return new Result { IsSuccess = true };
        }

        public static Result Fail(string errorMessage)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}