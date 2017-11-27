using NUnit.Framework;
using ConsoleAdapter;

namespace ConsoleAdapterTest
{
    [TestFixture]
    public class CsvConverterTest
    {
        private CsvConverter _converter;

        [SetUp]
        public void BeforeEachTest()
        {
            _converter = new CsvConverter();
        }

        [Test]
        public void CsvConverter_converts_column_a()
        {
            Assert.That(_converter.ConvertRow(new [] { "a" } ), Is.EqualTo("a"));
        }

        [Test]
        public void CsvConverter_converts_column_b()
        {
            Assert.That(_converter.ConvertRow(new[] { "b" }), Is.EqualTo("b"));
        }

        [Test]
        public void CsvConverter_converts_columns_a_b()
        {
            Assert.That(_converter.ConvertRow(new[] { "a", "b" }), Is.EqualTo("a;b"));
        }

        [Test]
        public void CsvConverter_converts_columns_a_b_c()
        {
            Assert.That(_converter.ConvertRow(new[] { "a", "b", "c" }), Is.EqualTo("a;b;c"));
        }

        [Test]
        public void CsvConverter_converts_using_custom_separator()
        {
            var converter = new CsvConverter("#");
            Assert.That(converter.ConvertRow(new[] { "a", "b" }), Is.EqualTo("a#b"));
        }
    }
}
