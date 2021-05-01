using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class GradebookEntryHelpers
    {
        public static GradebookEntryReferenceType GetGradebookEntryReference(this GradebookEntry gradebookEntry)
        {
            return new GradebookEntryReferenceType
            {
                GradebookEntryIdentity = new GradebookEntryIdentityType
                {
                    SectionReference = gradebookEntry.SectionReference,
                    DateAssigned = gradebookEntry.DateAssigned,
                    GradebookEntryTitle = gradebookEntry.GradebookEntryTitle
                }
            };
        }
    }
}
