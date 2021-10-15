namespace FeatureManagement.ImplementationSwitchExtensions
{
    internal interface IImplementationSwitchSettings<TService>
            where TService : class
    {
        string FeatureName { get; }
    }
}
