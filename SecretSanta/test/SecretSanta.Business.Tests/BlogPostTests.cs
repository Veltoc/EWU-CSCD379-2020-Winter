using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class BlogPostTests
    {
        [TestMethod]
        public void Create_BlogPost_Sucsess()
        {
            // arrange
            string title = "Sample Blog Post";
            const string content = "Hello my name is jndvajn;vjansvdkjnasd;v";
            const string author = "Me";

            // act
            BlogPost blogPost = new BlogPost(
                title,
                content,
                DateTime.Now,
                author);

            // assert
            Assert.AreEqual<string>(title, blogPost.Title, "Title value is unexpected");
            Assert.AreEqual<string>(content, blogPost.Content, "Content value is unexpected");
            Assert.AreEqual<string>(author, blogPost.Author, "Author value is unexpected");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_VerifyPropertiesAreNotNull_NotNull()
        {
            new BlogPost(null!, "<content>", DateTime.Now, "Inigo Montoya");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(BlogPost.Content))]
        [DataRow(nameof(BlogPost.Author))]
        public void Properties_AssignNull_ThrowArgumentNullException(string propertyName)
        {
            SetPropertyOnBlogPost(propertyName, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(nameof(BlogPost.Content), "")]
        [DataRow(nameof(BlogPost.Author), "   ")]
        [DataRow(nameof(BlogPost.Author), " \t")]
        public void Properties_AssignNull_ThrowArgumentException(
            string propertyName, string value)
        {
            SetPropertyOnBlogPost(propertyName, value);
        }

        private static void SetPropertyOnBlogPost(
            string propertyName, string? value)
        {
            BlogPost blogPost = new BlogPost(
                "Sample Title", "<content>", DateTime.Now, "Inigo Montoya");

            //Retrieve the property information based on the type
            System.Reflection.PropertyInfo propertyInfo
                = blogPost.GetType().GetProperty(propertyName)!;

            try
            {
                //Set the value of the property
                propertyInfo.SetValue(blogPost, value, new object[0]);
            }
            catch(System.Reflection.TargetInvocationException exception)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException!).Throw();
            }
        }
    }
}
