using MediatR;

namespace Application.UseCases;

public abstract class UseCase<T> : IRequest<T>
{

}
