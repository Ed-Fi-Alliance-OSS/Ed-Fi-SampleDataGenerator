using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using log4net;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Coordination
{
    public class SampleDataGenerationService
    {
        private static ILog _log = LogManager.GetLogger(typeof (SampleDataGenerationService));
        
        private readonly StudentDataGenerationCoordinator _studentDataGenerationCoordinator;
        private readonly IGlobalDataGeneratorConfigReader _globalDataGeneratorConfigReader;
        private readonly GlobalDataGeneratorConfigValidator _globalDataGeneratorConfigValidator;
        private readonly GlobalDataGenerationCoordinator _globalDataGenerationCoordinator;
        private readonly TemplatedDataGenerationCoordinator _templatedDataGenerationCoordinator;


        public SampleDataGenerationService() : this (new GlobalDataGenerationCoordinator(), new StudentDataGenerationCoordinator(), new TemplatedDataGenerationCoordinator(),  new GlobalDataGeneratorConfigReader(), new GlobalDataGeneratorConfigValidator())
        {
        }

        public SampleDataGenerationService(GlobalDataGenerationCoordinator globalDataGenerationCoordinator, StudentDataGenerationCoordinator studentDataGenerationCoordinator, TemplatedDataGenerationCoordinator templatedDataGenerationCoordinator,
            IGlobalDataGeneratorConfigReader globalDataGeneratorConfigReader, GlobalDataGeneratorConfigValidator globalDataGeneratorConfigValidator)
        {
            _globalDataGenerationCoordinator = globalDataGenerationCoordinator;
            _studentDataGenerationCoordinator = studentDataGenerationCoordinator;
            _templatedDataGenerationCoordinator = templatedDataGenerationCoordinator;
            _globalDataGeneratorConfigReader = globalDataGeneratorConfigReader;
            _globalDataGeneratorConfigValidator = globalDataGeneratorConfigValidator;
        }
        
        public void Run(ISampleDataGeneratorConfig config)
        {
            var interchangeDataGeneratorConfig = _globalDataGeneratorConfigReader.Read(config);
            ValidateConfiguration(interchangeDataGeneratorConfig);

            _log.Info("Starting data generation");
            _templatedDataGenerationCoordinator.Run(interchangeDataGeneratorConfig);
            var globalData = _globalDataGenerationCoordinator.Run(interchangeDataGeneratorConfig);
            _studentDataGenerationCoordinator.Run(interchangeDataGeneratorConfig, globalData);
        }

        private void ValidateConfiguration(GlobalDataGeneratorConfig config)
        {
            _log.Info("Validating runtime config");
            var validationResult = _globalDataGeneratorConfigValidator.Validate(config);
            if (!validationResult.IsValid)
            {
                foreach (var validationFailure in validationResult.Errors)
                {
                    _log.Error(validationFailure.ErrorMessage);
                }

                throw new Exception("Invalid configuration");
            }
            _log.Info("Runtime config validation complete");
        }
    }
}
