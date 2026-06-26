using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Cards.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Cards.Queries;

public class GetCardByIdQuery : IRequest<CardDto>
{
    public Guid Id { get; set; }

    public class Handler(ICardRepository cardRepository, IMapper mapper) : IRequestHandler<GetCardByIdQuery, CardDto>
    {
        private readonly ICardRepository _cardRepository = cardRepository;
         
        private readonly IMapper _mapper = mapper;
        public async Task<CardDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetCardByIdAsync(request.Id);
            if (card == null)
            {
                throw new Exception("Card not found.");
            }
            return _mapper.Map<CardDto>(card);

         }
    }
}
