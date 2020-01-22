using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Group_SetNameToNull_ThrowsArgumentNullException()
        {
            _ = new Group {
                Name = null!
            };
        }
    }
}
