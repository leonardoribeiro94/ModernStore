using System;

namespace ModernStore.Domain.Entities
{
    public class User
    {
        public User(string userName, string passWord)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            PassWord = passWord;
            Active = false;
        }

        public Guid Id { get; set; }
        public string UserName { get; private set; }
        public string PassWord { get; private set; }
        public bool Active { get; private set; }

        public void Activate() => Active = true;
        public void Desactvate() => Active = false;
    }
}
