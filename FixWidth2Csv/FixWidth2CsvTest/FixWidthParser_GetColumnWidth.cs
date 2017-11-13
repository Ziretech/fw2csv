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
    public class FixWidthParser_GetColumnWidth
    {
        private FixWidthParserOld _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new FixWidthParserOld();
        }

        [Test]
        public void FixWidthParser_read_column_with_width_1()
        {
            Assert.That(_parser.GetColumnWidths("-"), Is.EquivalentTo(new[] { 1 }));
        }

        [Test]
        public void FixWidthParser_read_column_with_width_2()
        {
            Assert.That(_parser.GetColumnWidths("--"), Is.EquivalentTo(new[] { 2 }));
        }

        [Test]
        public void FixWidthParser_read_column_with_width_1_1()
        {
            Assert.That(_parser.GetColumnWidths("- -"), Is.EquivalentTo(new[] { 1, 1 }));
        }

        [Test]
        public void FixWidthParser_read_column_with_width_3_7()
        {
            Assert.That(_parser.GetColumnWidths("--- -------"), Is.EquivalentTo(new[] { 3, 7 }));
        }

        [Test]
        public void FixWidthParser_throws_exception_when_space_between_delimiter_is_more_than_one_space()
        {
            try
            {
                _parser.GetColumnWidths("---  -------");
                Assert.Fail("No exception was thrown.");
            }
            catch(ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("more than one space"));
                Assert.That(exception.Message.ToLower(), Does.Contain("delimiter"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_delimiterline_is_empty()
        {
            try
            {
                _parser.GetColumnWidths("");
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no delimiters"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_delimiterline_is_null()
        {
            try
            {
                _parser.GetColumnWidths(null);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no delimiters"));
            }
        }

        [Test]
        public void FixWidthParser_throws_exception_when_delimiterline_is_only_space()
        {
            try
            {
                _parser.GetColumnWidths("     ");
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no delimiters"));
            }
        }
    }
}
