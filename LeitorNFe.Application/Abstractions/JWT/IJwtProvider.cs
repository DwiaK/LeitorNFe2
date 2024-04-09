namespace LeitorNFe.Application.Abstractions.JWT;

public interface IJwtProvider
{
	string Generate(); // TODO: utilizar usuário como parâmetro.
}
