using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Comment>> GetCommentsByCardIdAsync(Guid cardId)
    {
        return await _context.Comments.Where(x => x.CardId == cardId).Include(x => x.User).ToListAsync();
        
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
