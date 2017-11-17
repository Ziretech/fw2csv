using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;
using NUnit.Framework;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class ConvertFixWidthToMatrixSpec
    {
        private WriterMock _writer;
        private ReaderMock _reader;
        private ConvertFixWidthToMatrix _converter;

        [SetUp]
        public void Setup()
        {
            _writer = new WriterMock();
            _reader = new ReaderMock();
            _converter = new ConvertFixWidthToMatrix { Writer = _writer };
        }

        [Test]
        public void Converter_converts_simple_text_to_csv()
        {
            _reader.AddLine("a", 1);
            _reader.AddLine("-", 1);
            _reader.AddLine("b", 0);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new [] { "a" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new [] { "b" }));
        }

        [Test]
        public void Converter_converts_other_simple_text_to_csv()
        {
            _reader.AddLine("namn", 1);
            _reader.AddLine("----", 1);
            _reader.AddLine("ola", 0);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "namn" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "ola" }));
        }

        [Test]
        public void Converter_converts_2_column_text_to_csv()
        {
            _reader.AddLine("id   namn", 1);
            _reader.AddLine("---- ----", 1);
            _reader.AddLine("12   ola", 5);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "namn" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "12", "ola" }));
        }
        
        [Test]
        public void Converter_converts_2_data_rows_to_csv()
        {
            _reader.AddLine("id   namn", 1);
            _reader.AddLine("---- ----", 1);
            _reader.AddLine("12   ola", 5);
            _reader.AddLine("1245 sven", 5);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "namn" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "12", "ola" }));
            Assert.That(_writer.RowList[2], Is.EquivalentTo(new[] { "1245", "sven" }));
        }

        [Test]
        public void Converter_convert_column_with_width_2_2_2_and_middle_column_value_missing()
        {
            _reader.AddLine("id by ås", 1);
            _reader.AddLine("-- -- --", 1);
            _reader.AddLine("cg    a", 6);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "by", "ås" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "cg", "", "a" }));
        }

        [Test]
        public void Converter_convert_column_with_width_2_2_2_and_new_line_in_first_row_second_column_second_character()
        {
            _reader.AddLine("id by   å", 1);
            _reader.AddLine("-- ---- --", 1);
            _reader.AddLine("cg a\n   eg", 8);
            _reader.AddLine("e  fy   i", 8);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "by", "å" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "cg", "a\n", "eg" }));
            Assert.That(_writer.RowList[2], Is.EquivalentTo(new[] { "e", "fy", "i" }));
        }

        [Test]
        public void Converter_convert_column_with_width_2_2_2_and_cr_nl_in_first_row_second_column_second_character()
        {
            _reader.AddLine("id by   å", 1);
            _reader.AddLine("-- ---- --", 1);
            _reader.AddLine("cg a\r\n  eg", 8);
            _reader.AddLine("e  fy   i", 8);
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "by", "å" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "cg", "a\r\n", "eg" }));
            Assert.That(_writer.RowList[2], Is.EquivalentTo(new[] { "e", "fy", "i" }));
        }

        [Test]
        public void Converter_displays_which_row_was_processed_when_error_occurred()
        {
            _reader.AddLine("header", 1);
            _reader.AddLine("------", 1);
            _reader.AddLine("kort  ", 0);
            _reader.AddLine("kort", -1);

            try
            {
                _converter.Convert(_reader);
                Assert.Fail("No exception was thrown.");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("conversion error"));
                Assert.That(exception.Message.ToLower(), Does.Contain("4"));
            }
        }
    }
}
