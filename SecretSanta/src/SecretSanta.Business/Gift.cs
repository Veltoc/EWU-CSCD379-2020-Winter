using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{

    public class Gift
    {

        public Gift(int id, string title, string description, string url, User user)
        {
            Id = id;
            Title = title;
            Description = description ;
            Url = url;
            User = user;
        }
        public int Id { get; }
        private string _Title = "<Invalid>";
        public string Title
        {
            get => _Title;
            set => _Title = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
        }
        private string _Description = "<Invalid>";
        public string Description
        {
            get => _Description;
            set => _Description = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)):value;
        }
        private string _Url = "<Invalid>";
        public string Url
        {
            get => _Url;
            set => _Url = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
        }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public string Url { get; set; }
        public User User { get; set; }
    }
}
