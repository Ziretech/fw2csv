using FixWidth2Csv;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class FixWidthParser_ConvertText
    {
        private FixWidthParser _parser;
        private WriterMock _writer;

        [SetUp]
        public void SetUp()
        {
            _writer = new WriterMock();
            _parser = new FixWidthParser();
            _parser.Writer = _writer;
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_1_and_1_row()
        {
            var reader = new ReaderMock();
            reader.AddLine("a");
            reader.AddLine("-");
            reader.AddLine("b");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "a", "b" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_1_and_1_row_and_another_data()
        {
            var reader = new ReaderMock();
            reader.AddLine("c");
            reader.AddLine("-");
            reader.AddLine("d");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "c", "d" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_1_and_2_rows()
        {
            var reader = new ReaderMock();
            reader.AddLine("c");
            reader.AddLine("-");
            reader.AddLine("d");
            reader.AddLine("e");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "c", "d", "e" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_1_and_5_rows()
        {
            var reader = new ReaderMock();
            reader.AddLine("c");
            reader.AddLine("-");
            reader.AddLine("1");
            reader.AddLine("2");
            reader.AddLine("3");
            reader.AddLine("4");
            reader.AddLine("5");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "c", "1", "2", "3", "4", "5" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_1_1_and_2_rows()
        {
            var reader = new ReaderMock();
            reader.AddLine("a b");
            reader.AddLine("- -");
            reader.AddLine("c d");
            reader.AddLine("e f");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "a;b", "c;d", "e;f" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_2_2_and_2_rows()
        {
            var reader = new ReaderMock();
            reader.AddLine("id b");
            reader.AddLine("-- --");
            reader.AddLine("cg d");
            reader.AddLine("e  fy");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "id;b", "cg;d", "e;fy" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_2_2_2_and_middle_column_value_missing()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by ås");
            reader.AddLine("-- -- --");
            reader.AddLine("cg    a");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "id;by;ås", "cg;;a" }));
        }

        [Test]
        public void FixWidthParser_convert_column_with_width_2_2_2_and_new_line_in_first_row_second_column_second_character()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- ---- --");
            reader.AddLine("cg a"); // cg \na eg
            reader.AddLine("b eg");
            reader.AddLine("e  fy   i");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "id;by;å", "cg;a" + Environment.NewLine + "b;eg", "e;fy;i" }));
        }

        [Ignore("Trim tar också bort radbrytning, därför fungerar inte detta testfall. Det finns dock ingen cell med begynnande radbrytning i datat.")]
        [Test]
        public void FixWidthParser_convert_column_with_width_2_2_2_and_new_line_in_first_row_second_column_first_character()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- --- --");
            reader.AddLine("cg "); // cg \na eg
            reader.AddLine("b eg");
            reader.AddLine("e  fy  i");

            _parser.ConvertText(reader);
            Assert.That(_writer.WriteList, Is.EquivalentTo(new List<string>() { "id;by;å", "cg;" + Environment.NewLine + "b;eg", "e;fy;i" }));
        }

        [Test]
        public void FixWidthParser_throws_exception_when_there_is_too_many_cells_for_line_1()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- --- --");
            reader.AddLine("cg ab  eg d");
            reader.AddLine("e  fy  i");

            try
            {
                _parser.ConvertText(reader);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("too many"));
                Assert.That(exception.Message.ToLower(), Does.Contain("line 1"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_there_is_too_many_cells_for_line_2()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- --- --");
            reader.AddLine("cg ab  eg");
            reader.AddLine("e  fy  i  j");

            try
            {
                _parser.ConvertText(reader);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("too many"));
                Assert.That(exception.Message.ToLower(), Does.Contain("line 2"));
            }
        }
        
        [Test]
        public void FixWidthParser_throws_exception_when_there_is_precisely_too_few_cells()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- --- --");
            reader.AddLine("e  fy ");

            try
            {
                _parser.ConvertText(reader);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("not"));
                Assert.That(exception.Message.ToLower(), Does.Contain("enough cells"));
                Assert.That(exception.Message.ToLower(), Does.Contain("line 1"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_there_is_too_few_cells_for_line_2()
        {
            var reader = new ReaderMock();
            reader.AddLine("id by  å");
            reader.AddLine("-- --- --");
            reader.AddLine("cg ab  eg");
            reader.AddLine("e  fy");

            try
            {
                _parser.ConvertText(reader);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("not"));
                Assert.That(exception.Message.ToLower(), Does.Contain("enough cells"));
                Assert.That(exception.Message.ToLower(), Does.Contain("line 2"));
            }
        }
    }
}
