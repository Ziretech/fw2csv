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
    public class FixWidthParser_IsBrokenRow
    {
        private FixWidthParserOld _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new FixWidthParserOld();
        }

        [Test]
        public void FixWidthParser_determine_a_with_width_1_as_NOT_a_broken_row()
        {
            Assert.That(_parser.IsBrokenRow("a", new[] { 1 }), Is.False);
        }

        [Test]
        public void FixWidthParser_determine_a_b_with_width_1_1_as_NOT_a_broken_row()
        {
            Assert.That(_parser.IsBrokenRow("a b", new[] { 1, 1 }), Is.False);
        }

        [Test]
        public void FixWidthParser_determine_empty_string_with_width_1_1_as_a_broken_row()
        {
            Assert.That(_parser.IsBrokenRow("", new[] { 1, 1 }), Is.True);
        }

        [Test]
        public void FixWidthParser_determine_a_with_width_2_1_as_a_broken_row() // as second character in line is \n
        {
            Assert.That(_parser.IsBrokenRow("a", new[] { 2, 1 }), Is.True);
        }

        [Test]
        public void FixWidthParser_determine_ab_space_with_width_2_2_2_as_a_broken_row() // as first character in second cell is \n
        {
            Assert.That(_parser.IsBrokenRow("ab ", new[] { 2, 2, 2 }), Is.True); 
        }

        [Test]
        public void FixWidthParser_determine_ab_c_with_width_2_2_2_as_a_broken_row() // as second character in second cell is \n
        {
            Assert.That(_parser.IsBrokenRow("ab c", new[] { 2, 2, 2 }), Is.True);
        }

        [Test]
        public void FixWidthParser_determine_ab_cd_with_width_2_2_2_as_NOT_a_broken_row() // last cell i just empty
        {
            Assert.That(_parser.IsBrokenRow("ab cd ", new[] { 2, 2, 2 }), Is.False);
        }
    }
}
