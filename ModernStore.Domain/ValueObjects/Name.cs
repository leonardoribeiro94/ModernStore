using FluentValidator;

namespace ModernStore.Domain.ValueObjects
{
    public class Name : Notifiable
    {
        public Name() { }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            new ValidationContract<Name>(this)
                .IsRequired(x => x.FirstName, "Nome é Obrigatório")
                .HasMaxLenght(x => x.FirstName, 60)
                .HasMinLenght(x => x.FirstName, 3)
                .IsRequired(x => x.LastName, "Sobrenome é Obrigatório")
                .HasMaxLenght(x => LastName, 60)
                .HasMinLenght(x => x.LastName, 3);
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
