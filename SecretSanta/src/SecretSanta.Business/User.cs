using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    public class User
    {
        
        public User(int id, string firstName, string lastName, List<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;
           

        }
        public int Id { get; }
        private string _FirstName = "<Invalid>";
        public string FirstName
        {
            get => _FirstName;
            set => _FirstName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
        }
        private string _LastName = "<Invalid>";
        public string LastName
        {
            get => _LastName;
            set => _LastName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
        }
        public List<Gift> Gifts { get; set; } 
    }

}
