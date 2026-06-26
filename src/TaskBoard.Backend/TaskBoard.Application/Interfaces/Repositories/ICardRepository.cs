using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public  interface ICardRepository
{
    Task<Card?> GetCardByIdAsync(Guid cardId);
    Task<Card?> GetCardForUpdateAsync(Guid cardId);
    Task<List<Card>> GetCardsByPositionAsync(Guid columnId, Guid laneId, Guid exceptCardId);
    Task<int> GetNextOrderAsync(Guid columnId, Guid laneId);
    Task AddCard(Card card);
}
 
