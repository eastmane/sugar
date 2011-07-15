﻿using NUnit.Framework;

namespace Comsec.Sugar
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void TestHtmlDecode()
        {
            var decoded = "test&amp;&#32;".HtmlDecode();

            Assert.AreEqual("test& ", decoded);
        }

        [Test]
        public void TestHtmlDecodeNullValue()
        {
            var decoded = ((string) null).HtmlDecode();

            Assert.IsNull(decoded);
        }

        [Test]
        public void TestHtmlEncode()
        {
            var decoded = "test& ".HtmlEncode();

            Assert.AreEqual("test&amp; ", decoded);
        }

        [Test]
        public void TestHtmlEncodeNullValue()
        {
            var decoded = ((string)null).HtmlEncode();

            Assert.IsNull(decoded);
        }

        [Test]
        public void TestStartsWithAndIgnoreCase()
        {
            var value = "Bonjour";

            var result = value.StartsWith("BON", true);
            Assert.IsFalse(result);

            result = value.StartsWith("bon", false);
            Assert.IsTrue(result);

            result = value.StartsWith("jour", true);
            Assert.IsFalse(result);
        }

        [Test]
        public void TestSubStringEmptyString()
        {
            Assert.AreEqual(string.Empty, string.Empty.SubstringAfterChar("c"));
        }

        [Test]
        public void TestSubStringWithNonMatchingString()
        {
            Assert.AreEqual("banana", "banana".SubstringAfterChar("c"));
        }

        [Test]
        public void TestSubStringWithMatchingString()
        {
            Assert.AreEqual("ana", "banana".SubstringAfterChar("n"));
        }


        [Test]
        public void TestSubStringBeforeWithMatchingString()
        {
            Assert.AreEqual("ba", "banana".SubstringBeforeChar("n"));
        }

        [Test]
        public void TestSubStringBeforeLastWithMatchingString()
        {
            Assert.AreEqual("bana", "banana".SubstringBeforeLastChar("n"));
        }


        [Test]
        public void TestSubStringAfterLastChar()
        {
            Assert.AreEqual("ba", "ab-ban-ba".SubstringAfterLastChar("-"));
        }

        [Test]
        public void TestKeep()
        {
            Assert.AreEqual("12", "1234".Keep("12"));
        }

        [Test]
        public void TestKeepWhenNull()
        {
            Assert.AreEqual(string.Empty, ((string) null).Keep("12"));
        }

        [Test]
        public void TestIsNumeric()
        {
            Assert.IsTrue("1234".IsNumeric());
            Assert.IsFalse("Hello".IsNumeric());
        }
    }
}
