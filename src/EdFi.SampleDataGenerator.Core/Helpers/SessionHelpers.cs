using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class SessionHelpers
    {
        public static IEnumerable<Session> ForSchool(this IEnumerable<Session> sessions, ISchoolProfile schoolProfile)
        {
            return sessions.Where(s => s.SchoolReference.ReferencesSchool(schoolProfile));
        }

        public static IEnumerable<Session> StartedBefore(this IEnumerable<Session> sessions, DateTime targetDate)
        {
            return sessions.Where(s => s.BeginDate < targetDate);
        }

        public static string GenerateSessionId(this Session session)
        {
            return GenerateSessionId(session.SchoolYear, session.Term);
        }

        public static string GenerateSessionId(SchoolYearType schoolYear, TermDescriptor term)
        {
            return $"{schoolYear.ToCodeValue()}_{term.CodeValue}".Replace(' ', '_').Replace('-', '_');
        }

        public static string GenerateSessionId(SchoolYearType schoolYear, string term)
        {
            return $"{schoolYear.ToCodeValue()}_{term.ParseToCodeValue()}".Replace(' ', '_').Replace('-', '_');
        }

        public static SessionReferenceType GetSessionReferenceType(this Session session)
        {
            return new SessionReferenceType
            {
                SessionIdentity = new SessionIdentityType
                {
                    SchoolYear = session.SchoolYear,
                    SchoolReference = session.SchoolReference,
                    SessionName = session.SessionName
                }
            };
        }

        public static bool ReferencesSameSessionAs(this SessionReferenceType reference1, SessionReferenceType reference2)
        {
            return reference1 != null &&
                   reference2 != null &&
                   (
                       reference1.ReferencesSameEntityAs(reference2) ||
                       (
                           (
                               reference1.SessionIdentity != null && 
                               reference2.SessionIdentity != null && 
                               reference1.SessionIdentity.SchoolReference != null &&
                               reference2.SessionIdentity.SchoolReference != null && 
                               reference1.SessionIdentity.SchoolReference.ReferencesSameSchoolAs(reference2.SessionIdentity.SchoolReference) &&
                               reference1.SessionIdentity.SchoolYear == reference2.SessionIdentity.SchoolYear && 
                               reference1.SessionIdentity.SessionName == reference2.SessionIdentity.SessionName
                           ) ||
                           (
                               reference1.SessionLookup != null && 
                               reference2.SessionLookup != null && 
                               reference1.SessionLookup.SchoolReference != null && 
                               reference2.SessionLookup.SchoolReference != null &&
                               reference1.SessionLookup.SchoolReference.ReferencesSameSchoolAs(reference2.SessionLookup.SchoolReference) &&
                               reference1.SessionLookup.SchoolYear == reference2.SessionLookup.SchoolYear && 
                               reference1.SessionLookup.Term == reference2.SessionLookup.Term &&
                               reference1.SessionLookup.SessionName == reference2.SessionLookup.SessionName
                            )
                       )
                   );
        }

        public static bool ReferencesSession(this SessionReferenceType reference, Session session)
        {
            return reference != null &&
                session != null &&
                (
                    reference.References(session.id)
                    ||
                    (
                        reference.SessionIdentity?.SchoolReference != null &&
                        reference.SessionIdentity.SchoolReference.ReferencesSameSchoolAs(session.SchoolReference) &&
                        reference.SessionIdentity.SchoolYear == session.SchoolYear &&
                        reference.SessionIdentity.SessionName == session.SessionName
                    )
                    ||
                    (
                        reference.SessionLookup != null &&
                        reference.SessionLookup.SchoolReference.ReferencesSameSchoolAs(session.SchoolReference) &&
                        reference.SessionLookup.SchoolYear == session.SchoolYear &&
                        reference.SessionLookup.Term == session.Term
                    )
                );
        }

        public static DateRange AsDateRange(this Session session)
        {
            return new DateRange(session.BeginDate, session.EndDate);
        }

        public static bool BeganInDateRange(this Session session, DateRange dateRange)
        {
            return dateRange.Contains(session.BeginDate);
        }
    }
}