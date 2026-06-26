using System;
using System.Security.Claims;
 using MediatR;
using Microsoft.AspNetCore.Http;
 using TaskBoard.Application.User.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using AutoMapper;

namespace TaskBoard.Application.User.Queries;

public class GetCurrentUserQuery : IRequest<UserDto?>
{
    public class Handler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetCurrentUserQuery, UserDto?>{
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
             if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var user = await _userRepository.GetCurrentUserAsync(userId);
             return _mapper.Map<UserDto>(user);
        }
    }

}
