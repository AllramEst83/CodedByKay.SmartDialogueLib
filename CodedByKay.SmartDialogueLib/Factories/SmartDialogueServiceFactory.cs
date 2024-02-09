using CodedByKay.SmartDialogueLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodedByKay.SmartDialogueLib.Factories
{
    /// <summary>
    /// Factory for creating instances of SmartDialogueService.
    /// </summary>
    public class SmartDialogueServiceFactory : ISmartDialogueServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the SmartDialogueServiceFactory class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to create service instances.</param>
        public SmartDialogueServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Creates a new instance of SmartDialogueService.
        /// </summary>
        /// <returns>A new instance of SmartDialogueService.</returns>
        /// <remarks>
        /// Uses a service scope to create a new instance of SmartDialogueService.
        /// </remarks>
        public ISmartDialogueService Create()
        {
            // Using a service scope to create a new instance of SmartDialogueService
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ISmartDialogueService>();
        }
    }
}
