using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.CalendarGenerator.Console
{
    public class TermTemplate
    {
        public DateTime StartDate => GradingPeriods.Min(gp => gp.StartDate);
        public DateTime EndDate => GradingPeriods.Max(gp => gp.EndDate);

        public int TermNumber { get; set; }

        public List<GradingPeriodTemplate> GradingPeriods { get; set; } = new List<GradingPeriodTemplate>();
        public int TotalInstructionalDays => GradingPeriods.Sum(gp => gp.TotalInstructionalDays);
    }
}