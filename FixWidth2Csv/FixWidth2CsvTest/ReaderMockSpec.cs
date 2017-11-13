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
        [Test]
        public void ReaderMock_reads_character_a()
        {
            var reader = new ReaderMock();
            reader.AddLine("a");
            Assert.That(reader.ReadLine(1), Is.EqualTo("a"));
        }

        [Test]
        public void ReaderMock_reads_character_bc()
        {
            var reader = new ReaderMock();
            reader.AddLine("bc");
            Assert.That(reader.ReadLine(2), Is.EqualTo("bc"));
        }

        [Test]
        public void ReaderMock_reads_character_a_then_b()
        {
            var reader = new ReaderMock();
            reader.AddLine("a");
            reader.AddLine("b");
            Assert.That(reader.ReadLine(1), Is.EqualTo("a"));
            Assert.That(reader.ReadLine(1), Is.EqualTo("b"));
        }

        [Test]
        public void ReaderMock_throws_exception_when_expected_line_ab_is_shorter_than_requested_1()
        {
            var reader = new ReaderMock();
            reader.AddLine("ab");
            try
            {
                reader.ReadLine(1);
                Assert.Fail("No exception was made.");
            }
            catch (ArgumentException exception)
            {
                
                Assert.That(exception.Message.ToLower(), Does.Contain("shorter"));
                Assert.That(exception.Message.ToLower(), Does.Contain("1"));
                Assert.That(exception.Message.ToLower(), Does.Contain("ab"));
            }
        }

        [Test]
        public void ReaderMock_throws_exception_when_expected_line_abc_is_shorter_than_requested_2()
        {
            var reader = new ReaderMock();
            reader.AddLine("abc");
            try
            {
                reader.ReadLine(2);
                Assert.Fail("No exception was made.");
            }
            catch (ArgumentException exception)
            {

                Assert.That(exception.Message.ToLower(), Does.Contain("shorter"));
                Assert.That(exception.Message.ToLower(), Does.Contain("2"));
                Assert.That(exception.Message.ToLower(), Does.Contain("abc"));
            }
        }
    }
}
