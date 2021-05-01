using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class SectionHelpers
    {
        /*Since UniqueSectionCode is not actually unique, and there is no actual unique identifier in the current section data
        we will determine uniqueness by using the multi-part primary key that exists on the Section table in the 3.0 ODS

         [LocalCourseCode], [SchoolId], [SchoolYear], [SectionIdentifier], [SessionName]
        */
        public static int GetUniqueSectionIdentifier(this Section section)
        {
            return
                (
                    section.LocationSchoolReference.SchoolIdentity.SchoolId
                    + section.CourseOfferingReference.CourseOfferingIdentity.LocalCourseCode
                    + section.CourseOfferingReference.CourseOfferingIdentity.SessionReference.SessionIdentity.SchoolYear
                    + section.SectionIdentifier
                    + section.CourseOfferingReference.CourseOfferingIdentity.SessionReference.SessionIdentity.SessionName
                    + section.SequenceOfCourse
                    ).GetHashCode();
        }

        public static IEnumerable<Section> ForSchool(this IEnumerable<Section> sections, ISchoolProfile schoolProfile)
        {
            return sections.Where(s => s.LocationSchoolReference.ReferencesSchool(schoolProfile));
        }

        public static IEnumerable<Section> ForSession(this IEnumerable<Section> sections, Session session)
        {
            return sections.Where(s => s.CourseOfferingReference.ReferencesSession(session));
        } 

        public static Course GetCourse(this Section section, CourseLookupCache cache)
        {
            return cache.GetCourseFromSection(section);
        }
        
        public static Course GetCourse(this Section section, GlobalDataGeneratorConfig config)
        {
            return section.GetCourse(config.CourseLookupCache);
        }

        public static StudentSectionAssociationReferenceType GetStudentSectionAssociationReference(this StudentSectionAssociation studentSectionAssociation)
        {
            return new StudentSectionAssociationReferenceType
            {
                StudentSectionAssociationIdentity = new StudentSectionAssociationIdentityType
                {
                    BeginDate = studentSectionAssociation.BeginDate,
                    SectionReference = studentSectionAssociation.SectionReference,
                    StudentReference = studentSectionAssociation.StudentReference,
                }
            };
        }

        public static IEnumerable<Section> GetSectionsForStudent(this IEnumerable<Section> sections, IEnumerable<StudentSectionAssociation> studentSectionAssociations)
        {
            return sections.Where(s => studentSectionAssociations.Any(ssa => ssa.SectionReference.ReferencesSection(s)));
        } 

        public static StudentSectionAssociation GetStudentSectionForClassPeriod(this IEnumerable<StudentSectionAssociation> studentSectionAssociations, IEnumerable<Section> sections, int classPeriod)
        {
            studentSectionAssociations = studentSectionAssociations.ToList();

            var sectionForClassPeriod = sections.GetSectionsForStudent(studentSectionAssociations)
                    .First(s => s.ClassPeriodReference.First().GetNumericClassPeriod() == classPeriod);

            return sectionForClassPeriod != null
                ? studentSectionAssociations.First(ssa => ssa.SectionReference.ReferencesSection(sectionForClassPeriod))
                : null;
        }

        public static Section GetSection(this SectionReferenceType sectionReferenceType, StudentDataGeneratorConfig config)
        {
            return config.GlobalData.MasterScheduleData.Sections.FirstOrDefault(
                s => s.SectionIdentifier == sectionReferenceType.SectionIdentity?.SectionIdentifier || 
                s.SectionIdentifier == sectionReferenceType.SectionLookup?.SectionIdentifier );
        }

    }
}