using AutoMapper;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.AutoMapper
{
    public class AutoMapperCoreProfile : Profile
    {
        public AutoMapperCoreProfile()
        {
            CreateMap<GlobalDataGeneratorConfig, StudentDataGeneratorConfig>()
                .ForMember(x => x.GlobalData, m => m.Ignore())
                .ForMember(x => x.DistrictProfile, m => m.Ignore())
                .ForMember(x => x.SchoolProfile, m => m.Ignore())
                .ForMember(x => x.GradeProfile, m => m.Ignore())
                .ForMember(x => x.StudentProfile, m => m.Ignore());
        }
    }
}
