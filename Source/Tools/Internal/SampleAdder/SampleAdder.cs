using System;
using System.IO;

namespace SampleAdder
{
    internal static class SampleAdder
    {
        public static void AddSample(string sampleName)
        {
            var validationResult = SampleValidator.ValidateSampleName(sampleName);
            if (!validationResult.Ok)
                throw new ArgumentException(validationResult.Message, nameof(sampleName));

            var samplesDirectory = SampleLocator.GetSamplesDirectory();
            var sampleDirectory = Path.Combine(samplesDirectory, sampleName);
            Directory.CreateDirectory(sampleDirectory);

            TemplateService.InstantiateTemplate(sampleName, sampleDirectory);
        }
    }
}