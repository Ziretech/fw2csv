using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class ReaderMockOldSpec
    {
        private ReaderMockOld _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new ReaderMockOld();
        }

        [Test]
        public void ReaderMock_return_first_line()
        {
            _mock.AddLine("hej");
            Assert.That(_mock.ReadLine(), Is.EqualTo("hej"));
        }

        [Test]
        public void ReaderMock_returns_null_when_no_lines_are_added()
        {
            Assert.That(_mock.ReadLine(), Is.Null);
        }

        [Test]
        public void ReaderMock_return_null_for_second_readline_when_one_is_added()
        {
            _mock.AddLine("hej");
            _mock.ReadLine();
            Assert.That(_mock.ReadLine(), Is.Null);
        }

        [Test]
        public void ReaderMock_return_second_line_when_two_is_added()
        {
            _mock.AddLine("hej");
            _mock.AddLine("hej2");
            _mock.ReadLine();
            Assert.That(_mock.ReadLine(), Is.EqualTo("hej2"));
        }

        [Test]
        public void ReaderMock_return_third_line_when_three_is_added()
        {
            _mock.AddLine("hej");
            _mock.AddLine("hej2");
            _mock.AddLine("hej3");
            _mock.ReadLine();
            _mock.ReadLine();
            Assert.That(_mock.ReadLine(), Is.EqualTo("hej3"));
        }

        [Test]
        public void ReaderMock_return_third_line_when_four_is_added()
        {
            _mock.AddLine("hej");
            _mock.AddLine("hej2");
            _mock.AddLine("hej3");
            _mock.AddLine("hej4");
            _mock.ReadLine();
            _mock.ReadLine();
            Assert.That(_mock.ReadLine(), Is.EqualTo("hej3"));
        }
    }
}
