using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication;
using NUnit.Framework;

namespace ConsoleApplicationTest
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
            Assert.That(_converter.ConvertRow(new [] { "a" } ), Is.EqualTo("a" + Environment.NewLine));
        }

        [Test]
        public void CsvConverter_converts_column_b()
        {
            Assert.That(_converter.ConvertRow(new[] { "b" }), Is.EqualTo("b" + Environment.NewLine));
        }

        [Test]
        public void CsvConverter_converts_columns_a_b()
        {
            Assert.That(_converter.ConvertRow(new[] { "a", "b" }), Is.EqualTo("a;b" + Environment.NewLine));
        }

        [Test]
        public void CsvConverter_converts_columns_a_b_c()
        {
            Assert.That(_converter.ConvertRow(new[] { "a", "b", "c" }), Is.EqualTo("a;b;c" + Environment.NewLine));
        }
    }
}
