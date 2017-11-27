using NUnit.Framework;
using Entity;
using System.Linq;

namespace EntityTest
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

        [Test]
        public void Delimiters_return_minimum_required_width_0_of_1()
        {
            var delimiters = new Delimiters("-");
            Assert.That(delimiters.GetMinimumRequiredRowWidth(), Is.EqualTo(0));
        }

        [Test]
        public void Delimiters_return_minimum_required_width_0_of_7()
        {
            var delimiters = new Delimiters("-------");
            Assert.That(delimiters.GetMinimumRequiredRowWidth(), Is.EqualTo(0));
        }

        [Test]
        public void Delimiters_return_minimum_required_width_3_of_2_4()
        {
            var delimiters = new Delimiters("-- ----");
            Assert.That(delimiters.GetMinimumRequiredRowWidth(), Is.EqualTo(3));
        }

        [Test]
        public void Delimiters_return_minimum_required_width_8_of_2_4_1()
        {
            var delimiters = new Delimiters("-- ---- -");
            Assert.That(delimiters.GetMinimumRequiredRowWidth(), Is.EqualTo(8));
        }
    }
}
