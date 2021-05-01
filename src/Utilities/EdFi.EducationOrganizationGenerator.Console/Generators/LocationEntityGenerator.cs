using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class LocationEntityGenerator : EducationOrganizationEntityGenerator
    {
        public LocationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.Location;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.School, };

        private readonly RandomOption<int>[] _seatingCapacityOptions = 
        {
            new RandomOption<int>(25, 0.6),
            new RandomOption<int>(30, 0.3),
            new RandomOption<int>(40, 0.1)
        };

        private readonly Dictionary<string, int> _standardRooms = new Dictionary<string, int>
        {
            {"GYM", 500},
            {"LIBRARY", 50},
            {"CAFETERIA", 100},
            {"AUDITORIUM", 200},
        };

        protected override void GenerateCore(EducationOrganizationData context)
        {
            foreach (var school in context.Schools)
            {
                for (var roomNumber = 100; roomNumber < 200; ++roomNumber)
                {
                    var roomSeatingCapacity = _seatingCapacityOptions.GetRandomItemWithDistribution(RandomNumberGenerator);
                    var room = GenerateLocation(school, $"{roomNumber:D}", roomSeatingCapacity.Value - 5, roomSeatingCapacity.Value);

                    context.Locations.Add(room);
                }

                foreach (var roomId in _standardRooms.Keys)
                {
                    var seats = _standardRooms[roomId];
                    var room = GenerateLocation(school, roomId, seats, seats);

                    context.Locations.Add(room);
                }
            }
        }

        private Location GenerateLocation(School school, string roomNumber, int optimalSeats, int maximumSeats)
        {
            return new Location
            {
                id = $"LOCN_{school.SchoolId}-{roomNumber}",
                ClassroomIdentificationCode = roomNumber,
                SchoolReference = school.GetSchoolReference(),
                MaximumNumberOfSeats = maximumSeats,
                MaximumNumberOfSeatsSpecified = true,
                OptimalNumberOfSeats = optimalSeats,
                OptimalNumberOfSeatsSpecified = true
            };
        }
    }
}
