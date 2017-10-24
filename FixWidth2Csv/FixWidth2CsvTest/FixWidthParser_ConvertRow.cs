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
    public class FixWidthParser_ConvertRow
    {
        private FixWidthParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new FixWidthParser();
        }

        [Test]
        public void FixWidthParser_convert_column_a_with_width_1()
        {
            Assert.That(_parser.ConvertRow("a", new[] { 1 }), Is.EqualTo("a"));
        }

        [Test]
        public void FixWidthParser_convert_column_b_with_width_1()
        {
            Assert.That(_parser.ConvertRow("b", new[] { 1 }), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_convert_column_b_with_width_5()
        {
            Assert.That(_parser.ConvertRow("b", new[] { 5 }), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_convert_column_a_b_with_width_1_1()
        {
            Assert.That(_parser.ConvertRow("a b", new[] { 1, 1 }), Is.EqualTo("a;b"));
        }

        [Test]
        public void FixWidthParser_convert_column_a_with_width_2()
        {
            Assert.That(_parser.ConvertRow("a ", new[] { 2 }), Is.EqualTo("a"));
        }

        [Test]
        public void FixWidthParser_convert_column_a_b_with_width_2_1()
        {
            Assert.That(_parser.ConvertRow("a  b", new[] { 2, 1 }), Is.EqualTo("a;b"));
        }

        [Test]
        public void FixWidthParser_convert_column_a_b_with_width_2_2()
        {
            Assert.That(_parser.ConvertRow("a  b", new[] { 2, 2 }), Is.EqualTo("a;b"));
        }

        [Test]
        public void FixWidthParser_convert_column_abc_b_with_width_5_5()
        {
            Assert.That(_parser.ConvertRow("abc   b", new[] { 5, 5 }), Is.EqualTo("abc;b"));
        }

        [Test]
        public void FixWidthParser_convert_column_abc_abcde_with_width_5_5()
        {
            Assert.That(_parser.ConvertRow("abc   abcde", new[] { 5, 5 }), Is.EqualTo("abc;abcde"));
        }

        [Test]
        public void FixWidthParser_convert_column_abcde_abcde_with_width_5_5()
        {
            Assert.That(_parser.ConvertRow("abcde abcde", new[] { 5, 5 }), Is.EqualTo("abcde;abcde"));
        }

        [Test]
        public void FixWidthParser_convert_column_with_new_line()
        {
            Assert.That(_parser.ConvertRow("ab\nde abcde", new[] { 5, 5 }), Is.EqualTo("ab\nde;abcde"));
        }

        [Test]
        public void FixWidthParser_throws_exception_when_row_doesnt_contain_enough_cells()
        {
            try
            {
                _parser.ConvertRow("abde", new[] {5, 5});
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("not"));
                Assert.That(exception.Message.ToLower(), Does.Contain("enough cells"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_row_contain_too_many_cells()
        {
            try
            {
                _parser.ConvertRow("abde  hej   a", new[] { 5, 5 });
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("too many cells"));
            }
        }

        [Test]
        public void FixWidthParser_uses_right_curly_bracket_as_cell_delimiter()
        {
            var parser = new FixWidthParser { CellDelimiter = "{"};
            Assert.That(parser.ConvertRow("abde  hej", new[] { 5, 5 }), Is.EqualTo("abde{hej"));
        }

        // get remaining cells

        [Test]
        public void FixWidthParser_get_empty_string_as_remainder_of_a_with_width_1()
        {
            Assert.That(_parser.GetRemainingCells("a", 1), Is.EqualTo(""));
        }

        [Test]
        public void FixWidthParser_get_empty_string_as_remainder_of_a_with_width_2()
        {
            Assert.That(_parser.GetRemainingCells("a", 2), Is.EqualTo(""));
        }

        [Test]
        public void FixWidthParser_get_b_as_remainder_of_a_b_with_width_1()
        {
            Assert.That(_parser.GetRemainingCells("a b", 1), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_get_b_as_remainder_of_a_b_with_width_2()
        {
            Assert.That(_parser.GetRemainingCells("a  b", 2), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_get_bcd_as_remainder_of_a_bcd_with_width_5()
        {
            Assert.That(_parser.GetRemainingCells("a     bcd", 5), Is.EqualTo("bcd"));
        }

        // read cell
        [Test]
        public void FixWidthParser_read_cell_a_with_width_1()
        {
            Assert.That(_parser.GetCell("a", 1), Is.EqualTo("a"));
        }

        [Test]
        public void FixWidthParser_read_cell_b_with_width_1()
        {
            Assert.That(_parser.GetCell("b", 1), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_read_cell_b_with_width_2()
        {
            Assert.That(_parser.GetCell("b", 2), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_read_cell_b_with_width_5()
        {
            Assert.That(_parser.GetCell("b     ", 5), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_read_cell_b_c_with_width_5()
        {
            Assert.That(_parser.GetCell("b     c", 5), Is.EqualTo("b"));
        }

        [Test]
        public void FixWidthParser_read_empty_cell_with_width_5()
        {
            Assert.That(_parser.GetCell("     ", 5), Is.EqualTo(""));
        }

        [Test]
        public void FixWidthParser_read_empty_cell_followed_by_b_with_width_5()
        {
            Assert.That(_parser.GetCell("      b", 5), Is.EqualTo(""));
        }
    }
}
