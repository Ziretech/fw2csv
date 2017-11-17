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
            _reader.AddLine("a", 1);
            Assert.That(_reader.ReadLine(1), Is.EqualTo("a"));
        }

        [Test]
        public void ReaderMock_reads_character_bc()
        {
            _reader.AddLine("bc", 2);
            Assert.That(_reader.ReadLine(2), Is.EqualTo("bc"));
        }

        [Test]
        public void ReaderMock_reads_character_a_then_b()
        {
            _reader.AddLine("a", 1);
            _reader.AddLine("b", 1);
            Assert.That(_reader.ReadLine(1), Is.EqualTo("a"));
            Assert.That(_reader.ReadLine(1), Is.EqualTo("b"));
        }

        [Test]
        public void ReaderMock_throws_exception_when_expected_line_ab_is_shorter_than_requested_3()
        {
            _reader.AddLine("ab", 3);
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
        public void ReaderMock_throws_exception_when_expected_line_abc_is_shorter_than_requested_4()
        {
            _reader.AddLine("abc", 4);
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
        public void ReaderMock_throws_an_exception_when_min_3_characters_was_expected_and_min_2_was_requested()
        {
            _reader.AddLine("1  kalle", 3);
            try
            {
                _reader.ReadLine(2);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("minimum number of characters"));
                Assert.That(exception.Message.ToLower(), Does.Contain("3"));
                Assert.That(exception.Message.ToLower(), Does.Contain("2"));
            }
        }

        [Test]
        public void ReaderMock_have_more_lines()
        {
            _reader.AddLine("a", 0);
            Assert.That(_reader.MoreLines, Is.True);
        }

        [Test]
        public void ReaderMock_have_no_more_lines_after_last()
        {
            _reader.AddLine("a", 1);
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
