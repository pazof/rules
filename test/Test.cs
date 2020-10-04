// // Test.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 16:36 20202020 10 3
// */
using NUnit.Framework;
using rules;
using System;
using System.Linq;

namespace test
{
    [TestFixture()]
    public class Test
    {
        RuleSetParser parser;

        public Test()
        {
            parser = new RuleSetParser(false);
        }

        [Test()]
        public void TestWholeFamily()
        {
            parser.Reset();
            parser.Parse("FAMILY: @Paul, @Soraya, @Ninou, @Sherine;");
            parser.Parse("FAMNPLUONE: @Mom, @Gilles, @Julien;");
            parser.Parse("WHOLEFAMILY: #FAMILY, #FAMNPLUONE;");

            parser.Parse("allow from #WHOLEFAMILY;");
            Assert.IsTrue(parser.Rules.Deny("Joe"), "Joe is not denied.");
            Assert.IsFalse(parser.Rules.Deny("Mom"), "Mom is denied.");
            Assert.IsTrue(parser.Rules.Allow("Mom"), "Mom is not allowed.");
            Assert.IsFalse(parser.Rules.Allow("Joe"), "Joe is allowed.");
            Assert.IsFalse(parser.Rules.Deny("Soraya"), "Soraya is denied.");
            Assert.IsTrue(parser.Rules.Allow("Soraya"), "Soraya is not allowed.");
        }

        [Test()]
        public void TestNear()
        {
            parser.Reset();
            parser.Parse("FAMILY: @Paul, @Soraya, @Ninou,@Sherine;");
            parser.Parse("allow from #FAMILY;");
            Assert.IsTrue(parser.Rules.Deny("Joe"), "Joe is not denied.");
            Assert.IsFalse(parser.Rules.Allow("Joe"), "Joe is allowed.");
            Assert.IsTrue(parser.Rules.Deny("Mom"), "Mom is not denied.");
            Assert.IsFalse(parser.Rules.Allow("Mom"), "Mom is allowed.");
            Assert.IsFalse(parser.Rules.Deny("Ninou"), "Mom is denied.");
            Assert.IsTrue(parser.Rules.Allow("Ninou"), "Mom is not allowed.");
            Assert.IsFalse(parser.Rules.Deny("Soraya"), "Soraya is denied.");
            Assert.IsTrue(parser.Rules.Allow("Soraya"), "Soraya is not allowed.");
        }
    }
}
