using NUnit.Framework;
using FixWidth2Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class DelimitersSpec
    {
        [Test]
        public void Delimiters_returns_size_of_1()
        {
            var delimiters = new Delimiters("-");
            Assert.That(delimiters.GetColumnWidths().ToArray(), Is.EquivalentTo(new[] { 1 }));
        }

        [Test]
        public void Delimiters_returns_size_of_2()
        {
            var delimiters = new Delimiters("--");
            Assert.That(delimiters.GetColumnWidths().ToArray(), Is.EquivalentTo(new[] { 2 }));
        }

        [Test]
        public void Delimiters_returns_size_of_1_1()
        {
            var delimiters = new Delimiters("- -");
            Assert.That(delimiters.GetColumnWidths().ToArray(), Is.EquivalentTo(new[] { 1, 1 }));
        }

        [Test]
        public void Delimiters_returns_size_of_3_4()
        {
            var delimiters = new Delimiters("--- ----");
            Assert.That(delimiters.GetColumnWidths().ToArray(), Is.EquivalentTo(new[] { 3, 4 }));
        }
    }
}
