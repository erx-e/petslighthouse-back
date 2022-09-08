using System;
using System.ComponentModel.DataAnnotations;

namespace petsLighthouseAPI.Validations
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute()
          : base(typeof(DateTime), DateTime.Now.AddYears(-20).ToShortDateString(), DateTime.Now.ToShortDateString()) { }
    }
}
