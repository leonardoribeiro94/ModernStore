using ModernStore.Domain.ValueObjects;
using ModernStore.Shared.Entities;
using System;

namespace ModernStore.Domain.Entities
{
    public class Customer : Entity
    {
        public Customer(Name name, DateTime birthDay, Email email,
            Document document,
            User user)
        {
            Name = name;
            BirthDay = birthDay;
            Email = email;
            Document = document;
            User = user;

            AddNotifications(Name.Notifications);
            AddNotifications(Email.Notifications);
            AddNotifications(Document.Notifications);
        }

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public DateTime BirthDay { get; private set; }
        public Document Document { get; private set; }
        public User User { get; private set; }


        public void Update(Name name, DateTime birthDay)
        {
            Name = name;
            BirthDay = birthDay;
        }
    }
}
