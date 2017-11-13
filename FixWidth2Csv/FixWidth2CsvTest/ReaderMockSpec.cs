using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class ReaderMockSpec
    {
        private ReaderMock _reader;

        [SetUp]
        public void Setup()
        {
            _reader = new ReaderMock();
        }

        [Test]
        public void ReaderMock_reads_character_a()
        {
            _reader.AddLine("a");
            Assert.That(_reader.ReadLine(1), Is.EqualTo("a"));
        }

        [Test]
        public void ReaderMock_reads_character_bc()
        {
            _reader.AddLine("bc");
            Assert.That(_reader.ReadLine(2), Is.EqualTo("bc"));
        }

        [Test]
        public void ReaderMock_reads_character_a_then_b()
        {
            _reader.AddLine("a");
            _reader.AddLine("b");
            Assert.That(_reader.ReadLine(1), Is.EqualTo("a"));
            Assert.That(_reader.ReadLine(1), Is.EqualTo("b"));
        }

        [Test]
        public void ReaderMock_throws_exception_when_expected_line_ab_is_shorter_than_requested_1()
        {
            _reader.AddLine("ab");
            try
            {
                _reader.ReadLine(3);
                Assert.Fail("No exception was made.");
            }
            catch (ArgumentException exception)
            {
                
                Assert.That(exception.Message.ToLower(), Does.Contain("shorter"));
                Assert.That(exception.Message.ToLower(), Does.Contain("3"));
                Assert.That(exception.Message.ToLower(), Does.Contain("ab"));
            }
        }

        [Test]
        public void ReaderMock_throws_exception_when_expected_line_abc_is_shorter_than_requested_2()
        {
            _reader.AddLine("abc");
            try
            {
                _reader.ReadLine(4);
                Assert.Fail("No exception was made.");
            }
            catch (ArgumentException exception)
            {

                Assert.That(exception.Message.ToLower(), Does.Contain("shorter"));
                Assert.That(exception.Message.ToLower(), Does.Contain("4"));
                Assert.That(exception.Message.ToLower(), Does.Contain("abc"));
            }
        }

        [Test]
        public void ReaderMock_have_more_lines()
        {
            _reader.AddLine("a");
            Assert.That(_reader.MoreLines, Is.True);
        }

        [Test]
        public void ReaderMock_have_no_more_lines_after_last()
        {
            _reader.AddLine("a");
            _reader.ReadLine(1);
            Assert.That(_reader.MoreLines, Is.False);
        }

        [Test]
        public void ReaderMock_have_no_lines_when_no_was_available()
        {
            Assert.That(_reader.MoreLines, Is.False);
        }
    }
}
