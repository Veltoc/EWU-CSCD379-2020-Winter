using System;
using System.Collections.Generic;


namespace SecretSanta.Data
{
    public class User : FingerPrintEntityBase
    {
        
        public string FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(FirstName)); }
        private string _FirstName = string.Empty;
        public string LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(LastName)); }
        private string _LastName = string.Empty;
#nullable disable
        public ICollection<Gift> Gifts { get; set; }
        public List<UserGroup> UserGroups { get; set; }
#nullable enable
        // public Nullable<User> Santa { get; set; } nullable property? something may need to be changed to make this work. or its something else
        public User? Santa { get; set; } //this may make a it a nullable property... unsure.
    }
}
