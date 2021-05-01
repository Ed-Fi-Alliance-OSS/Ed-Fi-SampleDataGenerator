using System;
using System.Collections.Generic;

namespace EdFi.CalendarGenerator.Console
{
    public enum TermType
    {
        Semester
    }

    public class CalendarGeneratorConfig
    {
        public TermType TermType { get; set; }
        public int GradingPeriodLengthInWeeks { get; set; }
        public DateTime SchoolYearStartDate { get; set; }
        public string SchoolFilePath { get; set; }
        public List<string> SchoolIds { get; set; } 
        public int TeacherWorkdaysPerGradingPeriod { get; set; }
        public int BadWeatherDaysPerGradingPeriod { get; set; }
        public string OutputPath { get; set; }
    }
}
