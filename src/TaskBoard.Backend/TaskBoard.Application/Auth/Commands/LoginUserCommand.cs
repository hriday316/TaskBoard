using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Auth.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Auth.Commands;

public class LoginUserCommand : IRequest<UserDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class Handler(IAuthRepository authRepository, IMapper mapper) : IRequestHandler<LoginUserCommand, UserDto>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email) ?? throw new Exception("Invalid email or password");
            var result = await _authRepository.SignInAsync(request.Email, request.Password);
            if (!result.Succeeded)
                throw new Exception("Invalid email or password");
                
            return _mapper.Map<UserDto>(user);
        }
    }
}


