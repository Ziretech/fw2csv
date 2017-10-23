using FixWidth2Csv;
using NUnit.Framework;
using System;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class FixWidthParser_ConvertHeaders
    {
        private FixWidthParser _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new FixWidthParser();
        }

        [Test]
        public void FixWidthParser_converts_header_hej()
        {
            Assert.That(_parser.ConvertHeader("hej"), Is.EqualTo("hej"));
        }

        [Test]
        public void FixWidthParser_converts_header_id()
        {
            Assert.That(_parser.ConvertHeader("id"), Is.EqualTo("id"));
        }

        [Test]
        public void FixWidthParser_converts_header_id_hej()
        {
            Assert.That(_parser.ConvertHeader("id hej"), Is.EqualTo("id;hej"));
        }

        [Test]
        public void FixWidthParser_converts_header_id_hej_name()
        {
            Assert.That(_parser.ConvertHeader("id hej name"), Is.EqualTo("id;hej;name"));
        }

        [Test]
        public void FixWidthParser_converts_header_with_large_space()
        {
            Assert.That(_parser.ConvertHeader("id       hej"), Is.EqualTo("id;hej"));
        }

        [Test]
        public void FixWidthParser_throws_exception_when_headerline_is_empty()
        {
            try
            {
                _parser.ConvertHeader("");
                Assert.Fail("No exception was thrown.");
            }
            catch(ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no header"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_headerline_is_null()
        {
            try
            {
                _parser.ConvertHeader(null);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no header"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_headerline_only_contain_space()
        {
            try
            {
                _parser.ConvertHeader(" ");
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no header"));
            }
        }
    }
}
