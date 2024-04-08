using LeitorNFe.Application.Abstractions.Messaging;
using System.Reflection;

namespace LeitorNFe.API.Configurations;

public static class HandlersConfiguration
{
	public static void AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
	{
		#region Command Types
		var commandHandlerInterfaceType = typeof(ICommandHandler<>);
		var commandHandlerResponseInterfaceType = typeof(ICommandHandler<,>);
		#endregion

		#region Query Types
		var queryHandlerInterfaceType = typeof(IQueryHandler<,>);
		#endregion

		var handlerTypes = assembly.GetTypes()
			.Where(t => t.Namespace != null && t.Namespace.StartsWith("LeitorNFe.Application"))
			.Where(t => t.Name.EndsWith("Handler"))
			.Where(t => t.GetInterfaces().Any(i =>
				i.IsGenericType &&
				(i.GetGenericTypeDefinition() == commandHandlerInterfaceType ||
				 i.GetGenericTypeDefinition() == commandHandlerResponseInterfaceType ||
				 i.GetGenericTypeDefinition() == queryHandlerInterfaceType)))
			.ToList();

		foreach (var handlerType in handlerTypes)
		{
			var implementedInterfaces = handlerType.GetInterfaces().Where(i =>
				i.IsGenericType &&
				(i.GetGenericTypeDefinition() == commandHandlerInterfaceType ||
				 i.GetGenericTypeDefinition() == commandHandlerResponseInterfaceType ||
				 i.GetGenericTypeDefinition() == queryHandlerInterfaceType));

			foreach (var implementedInterface in implementedInterfaces)
			{
				services.AddTransient(implementedInterface, handlerType);
			}
		}
	}
}