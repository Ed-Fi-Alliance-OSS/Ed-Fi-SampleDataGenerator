using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.CalendarGenerator.Console
{
    public class SchoolYearTemplate
    {
        public DateTime StartDate => Terms.Min(gp => gp.StartDate);
        public DateTime EndDate => Terms.Max(gp => gp.EndDate);

        public List<TermTemplate> Terms { get; set; } = new List<TermTemplate>();
        public int TotalInstructionalDays => Terms.Sum(t => t.TotalInstructionalDays);
    }
}