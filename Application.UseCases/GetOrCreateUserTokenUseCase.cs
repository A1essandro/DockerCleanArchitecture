namespace Application.UseCases;

public class GetOrCreateUserTokenUseCase : UseCase<string>
{

    public GetOrCreateUserTokenUseCase(string email) => Email = email;

    public string Email { get; }

}
