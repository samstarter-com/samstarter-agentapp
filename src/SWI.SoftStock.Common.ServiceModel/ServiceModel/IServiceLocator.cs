namespace SWI.SoftStock.Common.ServiceModel
{
	/// <summary>
	/// Creates proxies for services
	/// </summary>
	public interface IServiceLocator
	{
		/// <summary>
		/// Creates proxy for service with provided contract
		/// </summary>
		/// <typeparam name="TService">
		/// Service contract
		/// </typeparam>
		TService GetServiceProxy<TService>();

		/// <summary>
		/// Creates proxy for service with provided contract
		/// </summary>
		/// <typeparam name="TService">
		/// Service contract
		/// </typeparam>
		TService GetServiceProxy<TService>(string serviceAddress);

		/// <summary>
		/// Gets endpoint description for provided service contract <see cref="TService"/>.
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <returns></returns>
		System.ServiceModel.Description.ServiceEndpoint GetServiceEndpoint<TService>();
	}

}
