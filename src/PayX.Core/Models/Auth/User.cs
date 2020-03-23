using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PayX.Core.Models.Auth
{
    public class User
    {
        public User()
        {
            Payments = new Collection<Payment>();
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}