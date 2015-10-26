using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class FeatureCleanupAttributeTests
    {
        [Test]
        public void Ctor_SetsName()
        {
            var attr = new FeatureCleanupAttribute("somename");

            Assert.AreEqual("somename", attr.Name);
        }
    }
}
