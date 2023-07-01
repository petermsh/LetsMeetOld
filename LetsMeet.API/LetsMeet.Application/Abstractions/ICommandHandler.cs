namespace LetsMeet.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult> where TResult : class
{
    Task<TResult> HandleAsync(TCommand command);
}