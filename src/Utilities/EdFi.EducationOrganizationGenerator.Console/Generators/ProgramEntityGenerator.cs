using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class ProgramEntityGenerator : EducationOrganizationEntityGenerator
    {
        public ProgramEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.Program;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.LocalEducationAgency, };

        private readonly ProgramTypeDescriptor[] _standardProgramTypes = 
        {
            ProgramTypeDescriptor.Athletics,
            ProgramTypeDescriptor.Bilingual,
            ProgramTypeDescriptor.BilingualSummer,
            ProgramTypeDescriptor.EnglishAsASecondLanguageESL,
            ProgramTypeDescriptor.GiftedAndTalented, 
            ProgramTypeDescriptor.RegularEducation,
            ProgramTypeDescriptor.SpecialEducation,
            ProgramTypeDescriptor.TitleIPartA,
        };

        protected override void GenerateCore(EducationOrganizationData context)
        {
            var localEducationAgency = context.LocalEducationAgencies.First();

            for (var i = 0; i < _standardProgramTypes.Length; ++i)
            {
                var programId = $"{localEducationAgency.LocalEducationAgencyId}_{(i + 1):D2}";
                var programType = _standardProgramTypes[i];

                var program = new SampleDataGenerator.Core.Entities.Program
                {
                    id = $"PRGM_{programId}",
                    ProgramId = programId,
                    EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(localEducationAgency.LocalEducationAgencyId),
                    ProgramType = programType.GetStructuredCodeValue(),
                    ProgramName = programType.GetStructuredCodeValue(),
                    ProgramSponsor = new [] { ProgramSponsorDescriptor.LocalEducationAgency.GetStructuredCodeValue()},
                };

                context.Programs.Add(program);
            }
        }
    }
}
