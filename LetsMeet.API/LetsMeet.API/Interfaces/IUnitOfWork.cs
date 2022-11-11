using System.Threading.Tasks.Dataflow;

namespace LetsMeet.API.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Complete();
    bool HasChanges();
}