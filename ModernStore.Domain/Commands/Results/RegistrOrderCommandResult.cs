using ModernStore.Shared.Commands;

namespace ModernStore.Domain.Commands.Results
{
    public class RegistrOrderCommandResult :
        ICommandResult
    {
        public RegistrOrderCommandResult()
        {

        }

        public RegistrOrderCommandResult(string number)
        {
            Number = number;
        }

        public string Number { get; set; }
    }
}
