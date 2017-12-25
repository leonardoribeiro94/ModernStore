using FluentValidator;
using ModernStore.Shared.Entities;
using System.Text;

namespace ModernStore.Domain.Entities
{
    public class User : Entity
    {
        public User(string userName, string passWord, string confirmPassword)
        {

            UserName = userName;
            PassWord = EncryptPassword(passWord);
            Active = false;

            new ValidationContract<User>(this)
                .AreEquals(x => x.PassWord, EncryptPassword(confirmPassword), "As senhas não coincidem");
        }

        public string UserName { get; private set; }
        public string PassWord { get; private set; }
        public bool Active { get; private set; }

        public void Activate() => Active = true;
        public void Desactvate() => Active = false;


        private string EncryptPassword(string pass)
        {
            if (string.IsNullOrEmpty(pass)) return "";
            var password = (pass += "|2d331cca-f6c0-40c0-bb43-6e32989c2881");
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString();
        }
    }

}
