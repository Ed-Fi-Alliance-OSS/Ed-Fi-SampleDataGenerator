namespace EdFi.SampleDataGenerator.Core.Entities
{
    public struct LanguageMapType
    {
        public string Value { get; private set; }

        public static readonly LanguageMapType English = new LanguageMapType { Value = "English" };
        public static readonly LanguageMapType Spanish = new LanguageMapType { Value = "Spanish" };
        public static readonly LanguageMapType Other  = new LanguageMapType { Value = "Other" };

        public bool Equals(LanguageMapType other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LanguageMapType && Equals((LanguageMapType)obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public static implicit operator LanguageMapType(string value)
        {
            return new LanguageMapType
            {
                Value = value
            };
        }

        public static bool operator ==(LanguageMapType lhs, string rhs)
        {
            return lhs.Value.Equals(rhs);
        }

        public static bool operator !=(LanguageMapType lhs, string rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(string lhs, LanguageMapType rhs)
        {
            return rhs.Value.Equals(lhs);
        }

        public static bool operator !=(string lhs, LanguageMapType rhs)
        {
            return !(lhs == rhs);
        }
    }

    public struct LanguageUseType
    {
        public string Value { get; private set; }

        public static readonly LanguageUseType Homelanguage = new LanguageUseType { Value = "Homelanguage" };
        public static readonly LanguageUseType Correspondencelanguage = new LanguageUseType { Value = "Correspondencelanguage" };
        public static readonly LanguageUseType Otherlanguageproficiency = new LanguageUseType { Value = "Otherlanguageproficiency" };

        public bool Equals(LanguageUseType other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LanguageUseType && Equals((LanguageUseType)obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(LanguageUseType lhs, LanguageUseType rhs)
        {
            return lhs.Value.Equals(rhs.Value);
        }

        public static bool operator !=(LanguageUseType lhs, LanguageUseType rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(LanguageUseType lhs, string rhs)
        {
            return lhs.Value.Equals(rhs);
        }

        public static bool operator !=(LanguageUseType lhs, string rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(string lhs, LanguageUseType rhs)
        {
            return rhs.Value.Equals(lhs);
        }

        public static bool operator !=(string lhs, LanguageUseType rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator string(LanguageUseType item)
        {
            return item.Value;
        }
    }

    public abstract class DescriptorReferenceType
    {
        public string CodeValue { get; set; }
        public string Namespace { get; set; }

        public static implicit operator string(DescriptorReferenceType descriptorReferenceType)
        {
            return descriptorReferenceType.CodeValue;
        }
    }

    public class LanguageDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class ProgramAssignmentDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class AcademicSubjectDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class AdministrativeFundingControlDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class CalendarEventDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class CourseIdentificationSystemDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class GradeLevelDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class GradingPeriodDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class TermDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class StaffIdentificationSystemDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class StaffClassificationDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class PerformanceLevelDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class LevelOfEducationDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class GraduationPlanTypeDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class EducationOrganizationIdentificationSystemDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class CompetencyLevelDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class AssessmentIdentificationSystemDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class AssessmentCategoryDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class ClassroomPositionDescriptorReferenceType : DescriptorReferenceType
    {
    }

    public class CountryDescriptorReferenceType : DescriptorReferenceType
    {
    }
}
