using LeitorNFe.Application.Abstractions.Messaging;
using System.Reflection;

namespace LeitorNFe.API.Configurations;

public static class HandlersConfiguration
{
	public static void AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
	{
		var handlerInterfaceType = typeof(ICommandHandler<>);
		var queryHandlerInterfaceType = typeof(IQueryHandler<,>);

		var handlerTypes = assembly.GetTypes()
			.Where(t => t.Namespace != null && t.Namespace.StartsWith("LeitorNFe.Application"))
			.Where(t => t.Name.EndsWith("Handler"))
			.Where(t => t.GetInterfaces().Any(i =>
				i.IsGenericType &&
				(i.GetGenericTypeDefinition() == handlerInterfaceType ||
				 i.GetGenericTypeDefinition() == queryHandlerInterfaceType)))
			.ToList();

		foreach (var handlerType in handlerTypes)
		{
			var implementedInterfaces = handlerType.GetInterfaces().Where(i =>
				i.IsGenericType &&
				(i.GetGenericTypeDefinition() == handlerInterfaceType ||
				 i.GetGenericTypeDefinition() == queryHandlerInterfaceType));

			foreach (var implementedInterface in implementedInterfaces)
			{
				services.AddTransient(implementedInterface, handlerType);
			}
		}
	}
}