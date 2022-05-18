using Application.UseCases;
using MediatR;

namespace Application.UseCaseHandlers;

public abstract class UseCaseHandler<TUseCase, T> : IRequestHandler<TUseCase, T> where TUseCase : UseCase<T>
{
    public abstract Task<T> Handle(TUseCase request, CancellationToken cancellationToken);

}
