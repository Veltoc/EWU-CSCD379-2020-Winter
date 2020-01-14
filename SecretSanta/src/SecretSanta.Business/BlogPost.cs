using System;

namespace SecretSanta.Business
{
    public class BlogPost
    {
        public BlogPost(string title, string content, DateTime now, string author)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content;
            Author = author;
        }

        public string Title { get; }

        private string _Content = "<Invalid>";
        public string Content
        {
            get => _Content;
            set => _Content = AssertIsNotNullOrWhitespace(value); 
        }
        private string _Author = "<Invalid>";
        public string Author
        {
            get => _Author;
            set => _Author = AssertIsNotNullOrWhitespace(value);
        }

        private string AssertIsNotNullOrWhitespace(string value) =>
            value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                "" => throw new ArgumentException($"{nameof(value)} cannot be an empty string.", nameof(value)),
                string temp when string.IsNullOrWhiteSpace(temp) => 
                    throw new ArgumentException($"{nameof(value)} cannot be only whitespace.", nameof(value)),
                _ => value
            };

    }
}