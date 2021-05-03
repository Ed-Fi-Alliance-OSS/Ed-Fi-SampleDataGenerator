using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console.Generators
{
    public class CourseOfferingEntityGenerator : MasterScheduleEntityGenerator
    {
        public CourseOfferingEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => MasterScheduleEntity.CourseOffering;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;

        protected override void GenerateCore(MasterScheduleData context)
        {
            foreach (var school in Configuration.Schools)
            {
                var availableSessions = Configuration.Sessions.Where(s => s.SchoolReference.SchoolIdentity.SchoolId == school.SchoolId);
                foreach (var session in availableSessions)
                {
                    var availableCourses = Configuration.Courses.Where(c => c.EducationOrganizationReference.EducationOrganizationIdentity.EducationOrganizationId == school.SchoolId);

                    foreach (var course in availableCourses)
                    {
                        var courseOffering = new CourseOffering
                        {
                            LocalCourseCode = course.CourseCode,
                            SchoolReference = school.GetSchoolReference(),
                            SessionReference = new SessionReferenceType
                            {
                                SessionIdentity = new SessionIdentityType
                                {
                                    SchoolReference = school.GetSchoolReference(),
                                    SchoolYear = Configuration.SchoolYear,
                                    SessionName = session.SessionName
                                },
                                SessionLookup = new SessionLookupType
                                {
                                    SchoolReference = school.GetSchoolReference(),
                                    SchoolYear = Configuration.SchoolYear,
                                    SessionName = session.SessionName,
                                    Term = session.Term
                                }
                            },
                            CourseReference = new CourseReferenceType
                            {
                                CourseIdentity = new CourseIdentityType
                                {
                                    CourseCode = course.CourseCode,
                                    EducationOrganizationReference = course.EducationOrganizationReference,
                                },
                                CourseLookup = new CourseLookupType
                                {
                                    CourseCode = course.CourseCode,
                                    EducationOrganizationReference = course.EducationOrganizationReference,
                                    CourseIdentificationCode = new CourseIdentificationCode
                                    {
                                        IdentificationCode = course.CourseCode,
                                        CourseIdentificationSystem = CourseIdentificationSystemDescriptor.LEACourseCode.GetStructuredCodeValue()
                                    }
                                }
                            }

                        };

                        context.CourseOfferings.Add(courseOffering);
                    }
                }
            }
        }
    }
}
