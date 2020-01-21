using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Group_SetNameToNull_ThrowsArgumentNullException()
        {
            var group = new Group
            {
                Name = null!
            };
        }
    }
}
