using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console.Generators
{
    public class SectionEntityGenerator : MasterScheduleEntityGenerator
    {
        public SectionEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => MasterScheduleEntity.Section;
        public override IEntity[] DependsOnEntities => new IEntity[] { MasterScheduleEntity.CourseOffering };

        protected override void GenerateCore(MasterScheduleData context)
        {
            foreach (var courseOfferingBySchool in context.CourseOfferings.GroupBy(co => co.SchoolReference.SchoolIdentity.SchoolId))
            {
                var schoolId = courseOfferingBySchool.Key;
                var school = Configuration.Schools.Single(s => s.SchoolId == schoolId);
                var localEducationAgencyId = school.LocalEducationAgencyReference.LocalEducationAgencyIdentity.LocalEducationAgencyId;

                var classPeriods = Configuration.ClassPeriods.Where(cp => cp.SchoolReference.SchoolIdentity.SchoolId == schoolId);
                var classRooms = Configuration.Locations.Where(l => l.SchoolReference.SchoolIdentity.SchoolId == schoolId).ToList();

                foreach (var classPeriod in classPeriods)
                {
                    var classRoomIndex = 0;
                    foreach (var courseOffering in courseOfferingBySchool)
                    {
                        var classRoom = classRooms[classRoomIndex];
                        classRoomIndex = (classRoomIndex + 1) % classRooms.Count;

                        var section = new Section
                        {
                            SectionIdentifier = GetSectionIdentifier(schoolId, classPeriod, classRoom, courseOffering),
                            SequenceOfCourse = 1,
                            EducationalEnvironment = EducationalEnvironmentDescriptor.Classroom.GetStructuredCodeValue(),
                            AvailableCredits = new Credits { Credits1 = 1, },
                            CourseOfferingReference = new CourseOfferingReferenceType
                            {
                                CourseOfferingIdentity = new CourseOfferingIdentityType
                                {
                                    LocalCourseCode = courseOffering.LocalCourseCode,
                                    SchoolReference = courseOffering.SchoolReference,
                                    SessionReference = courseOffering.SessionReference,
                                }
                            },
                            LocationSchoolReference = courseOffering.SchoolReference,
                            LocationReference = new LocationReferenceType
                            {
                                LocationIdentity = new LocationIdentityType
                                {
                                    ClassroomIdentificationCode = classRoom.ClassroomIdentificationCode,
                                    SchoolReference = classRoom.SchoolReference
                                }
                            },
                            ClassPeriodReference = new[]
                            {
                                new ClassPeriodReferenceType {
                                    ClassPeriodIdentity = new ClassPeriodIdentityType
                                    {
                                        ClassPeriodName = classPeriod.ClassPeriodName,
                                        SchoolReference = classPeriod.SchoolReference
                                    }
                                }
                            },
                            InstructionLanguage = LanguageDescriptor.English_eng.GetStructuredCodeValue(),
                            ProgramReference = new[]
                            {
                                new ProgramReferenceType
                                {
                                    ProgramIdentity = new ProgramIdentityType
                                    {
                                        EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(localEducationAgencyId),
                                        ProgramType = ProgramTypeDescriptor.RegularEducation.GetStructuredCodeValue(),
                                        ProgramName = ProgramTypeDescriptor.RegularEducation.GetStructuredCodeValue()
                                    }
                                }
                            },
                        };

                        context.Sections.Add(section);
                    }
                }
            }
        }

        private string GetSectionIdentifier(int schoolId, ClassPeriod classPeriod, Location classRoom, CourseOffering courseOffering)
        {
            return $"{schoolId}_{classPeriod.ClassPeriodName.Replace(" ", "")}_{courseOffering.LocalCourseCode.Replace("-", "")}_{classRoom.ClassroomIdentificationCode}_{Configuration.SchoolYear.ToCodeValue().Replace("-", "")}";
        }
    }
}
