using System.Collections.Generic;
using log4net;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class SampleDataGeneratorConfigValidator
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (SampleDataGeneratorConfigValidator));
        private readonly List<string> _validationErrors = new List<string>(); 
        private readonly Core.Config.SampleDataGeneratorConfigValidator _validator = new Core.Config.SampleDataGeneratorConfigValidator();

        public List<string> Validate(SampleDataGeneratorConfig config)
        {
            _validationErrors.Clear();

            _log.Info("Validating static configuration");

            var validationResult = _validator.Validate(config);

            if (!validationResult.IsValid)
            {
                foreach (var validationFailure in validationResult.Errors)
                {
                    _validationErrors.Add(validationFailure.ErrorMessage);
                    _log.Error(validationFailure.ErrorMessage);
                }
            }

            _log.Info("Static configuration validation complete");

            return _validationErrors;
        }
    }
}
