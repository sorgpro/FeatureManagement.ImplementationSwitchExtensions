namespace FeatureManagement.ImplementationSwitchExtensions
{
    internal class ImplementationSwitchSettings<TService, TFeatureImplementationOn, TFeatureImplementationOff>
        : IImplementationSwitchSettings<TService>
        where TService : class
        where TFeatureImplementationOn : class, TService
        where TFeatureImplementationOff : class, TService
    {

        public ImplementationSwitchSettings(string featureName)
        {
            FeatureName = featureName;
        }

        public string FeatureName { get; }
    }
}
