﻿using Application.UseCases;
using Core.Domain;
using Core.Specifications;
using Infrastructure.Contracts;
using Infrastructure.Contracts.Repositories;
using MediatR;

namespace Application.UseCaseHandlers;

internal class GetOrCreateUserTokenUseCaseHandler : IRequestHandler<GetOrCreateUserTokenUseCase, string>
{

    private readonly IRepository<User> _repo;
    private readonly IJsonWebTokenService _jwtService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetOrCreateUserTokenUseCaseHandler(IRepository<User> repo, IJsonWebTokenService jwtService, IDateTimeProvider dateTimeProvider)
    {
        _repo = repo;
        _jwtService = jwtService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<string> Handle(GetOrCreateUserTokenUseCase request, CancellationToken cancellationToken)
    {
        var user = await _repo.Get(new ByEmailSpec(request.Email), cancellationToken);

        var expiredTime = _dateTimeProvider.UtcNow.Subtract(_jwtService.Lifetime);

        if (user.Sessions.Any(x => x.Created > expiredTime))
        {
            var session = user.Sessions.First(x => x.Created > expiredTime);

            return session.Token;
        }

        var token = _jwtService.GetToken(user);

        user.Sessions.Add(new Session
        {
            Token = token
        });
        await _repo.Update(user, cancellationToken);

        return token;
    }

}
