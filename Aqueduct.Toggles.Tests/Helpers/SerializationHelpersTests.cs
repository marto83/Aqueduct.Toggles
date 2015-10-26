using System.Collections.Generic;
using System.Runtime.Serialization;
using Aqueduct.Toggles.Helpers;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests.Helpers
{
    [TestFixture]
    public class SerializationHelpersTests
    {
        [Test]
        public void CanSerialize_TestObject()
        {
            var test = new TestObject
                       {
                           ThisShouldBeSerialized = "test1",
                           ThisShouldNotBeSerialized = "test2"
                       };

            var output = test.Serialize();

            StringAssert.Contains("test1", output);
            StringAssert.DoesNotContain("test2", output);
        }

        [Test]
        public void CanDeserialize_TestObject()
        {
            var test = "<SerializationHelpersTests.TestObject xmlns=\"http://schemas.datacontract.org/2004/07/Aqueduct.Toggles.Tests.Helpers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><ThisShouldBeSerialized>test3</ThisShouldBeSerialized></SerializationHelpersTests.TestObject>";

            var output = test.Deserialize<TestObject>();

            Assert.IsNotNull(output);
            StringAssert.AreEqualIgnoringCase("test3", output.ThisShouldBeSerialized);
            Assert.IsNullOrEmpty(output.ThisShouldNotBeSerialized);
        }

        [Test]
        public void CanSerialize_AndDeserialize_Dictionary()
        {
            var dict = new Dictionary<string, bool>
                       {
                           {"key1", true},
                           {"key2", false},
                           {"key3", true}
                       };

            var output = dict.Serialize().Deserialize<IDictionary<string, bool>>();

            Assert.AreEqual(3, output.Count);
        }

        [DataContract]
        private class TestObject
        {
            [DataMember]
            public string ThisShouldBeSerialized { get; set; }
            public string ThisShouldNotBeSerialized { get; set; }
        }
    }
}
