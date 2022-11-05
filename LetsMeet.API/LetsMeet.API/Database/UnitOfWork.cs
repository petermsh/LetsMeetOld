using AutoMapper;
using AutoMapper.Configuration;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        _context.ChangeTracker.DetectChanges();
        var changes = _context.ChangeTracker.HasChanges();

        return changes;
    }
}