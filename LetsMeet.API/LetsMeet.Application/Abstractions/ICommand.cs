namespace LetsMeet.Application.Abstractions;

public interface ICommand
{
}

public interface ICommand<TResult> where TResult : class
{
}