using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface ILaneRepository
{
    Task AddLaneAsync(Lane lane);
    Task<Lane?> GetLaneByIdAsync(Guid laneId);
    Task<List<Lane>> GetLanesByBoardIdAsync(Guid boardId);
}
