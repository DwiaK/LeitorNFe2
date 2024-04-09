using LeitorNFe.Application.Abstractions.Command;
using System;

namespace LeitorNFe.Application.Authentication.Register;

public sealed record RegisterCommand(string email, string password) : ICommand<Guid>;
