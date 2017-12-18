using ModernStore.Shared.Commands;

namespace ModernStore.Domain.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }
}
