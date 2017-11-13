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
    public class ConvertFixWidthToCsvSpec
    {
        private WriterMock _writer;
        private ReaderMock _reader;
        private ConvertFixWidthToCsv _converter;

        [SetUp]
        public void Setup()
        {
            _writer = new WriterMock();
            _reader = new ReaderMock();
            _converter = new ConvertFixWidthToCsv { Writer = _writer };
        }

        [Test]
        public void Converter_converts_simple_text_to_csv()
        {
            _reader.AddLine("a");
            _reader.AddLine("-");
            _reader.AddLine("b");
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new [] { "a" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new [] { "b" }));
        }

        [Test]
        public void Converter_converts_other_simple_text_to_csv()
        {
            _reader.AddLine("namn");
            _reader.AddLine("----");
            _reader.AddLine("ola");
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "namn" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "ola" }));
        }

        [Test]
        [Ignore("Test class Delimiters first")]
        public void Converter_converts_2_column_text_to_csv()
        {
            _reader.AddLine("id   namn");
            _reader.AddLine("---- ----");
            _reader.AddLine("12   ola");
            _converter.Convert(_reader);

            Assert.That(_writer.RowList[0], Is.EquivalentTo(new[] { "id", "namn" }));
            Assert.That(_writer.RowList[1], Is.EquivalentTo(new[] { "12", "ola" }));
        }
    }
}
