using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public  interface ICommentRepository
{
    Task<Comment> CreateCommentAsync(Comment comment);
    Task<bool> DeleteCommentAsync(Guid commentId);
    Task<List<Comment>> GetCommentsByCardIdAsync(Guid cardId);
    Task SaveChangesAsync();

}
