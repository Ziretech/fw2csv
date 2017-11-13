using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class ReaderMockTest
    {
        private ReaderMock _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new ReaderMock();
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

        [Test]
        public void ReaderMock_return_line_h_when_reading_1_character()
        {
            _mock.AddLine("h");
            Assert.That(_mock.ReadCharacters(1), Is.EqualTo("h"));
        }

        [Test]
        public void ReaderMock_return_line_a_when_reading_1_character()
        {
            _mock.AddLine("a");
            Assert.That(_mock.ReadCharacters(1), Is.EqualTo("a"));
        }

        [Test]
        public void ReaderMock_return_line_a_when_reading_1_character_after_previous_reading()
        {
            _mock.AddLine("b");
            _mock.AddLine("a");
            _mock.ReadLine();
            Assert.That(_mock.ReadCharacters(1), Is.EqualTo("a"));
        }

        [Test]
        public void ReaderMock_return_line_a_when_reading_3_character()
        {
            _mock.AddLine("abc");
            Assert.That(_mock.ReadCharacters(3), Is.EqualTo("abc"));
        }

        [Test]
        public void ReaderMock_return_null_when_no_lines_are_available()
        {
            Assert.That(_mock.ReadCharacters(3), Is.Null);
        }

        [Test]
        public void ReaderMock_return_null_when_no_more_lines_are_available()
        {
            _mock.AddLine("a");
            _mock.ReadCharacters(1);
            Assert.That(_mock.ReadCharacters(2), Is.Null);
        }

        [Test]
        public void ReaderMock_return_2_characters_from_string_with_4_characters()
        {
            _mock.AddLine("abcd");
            Assert.That(_mock.ReadCharacters(2), Is.EqualTo("ab"));
        }

        [Test]
        public void ReaderMock_return_remaining_2_characters_from_string_with_4_characters_after_2_have_been_read()
        {
            _mock.AddLine("abcd");
            _mock.ReadCharacters(2);
            Assert.That(_mock.ReadCharacters(2), Is.EqualTo("cd"));
        }

        [Test]
        public void ReaderMock_return_remaining_characters_from_string_with_4_characters_after_2_have_been_read()
        {
            _mock.AddLine("abcd");
            _mock.ReadCharacters(2);
            Assert.That(_mock.ReadLine(), Is.EqualTo("cd"));
        }
    }
}
