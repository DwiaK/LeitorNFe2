using LeitorNFe.Application.Abstractions.Command;

namespace LeitorNFe.Application.Authentication.Login;

public sealed record LoginCommand(string email, string password) : ICommand<string>;
