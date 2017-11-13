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
    public class RowsSpec
    {
        // GetCells

        [Test]
        public void Rows_reads_1_cell()
        {
            var rows = new Rows("a", new[] { 1 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "a" }));
        }

        [Test]
        public void Rows_reads_2_cell()
        {
            var rows = new Rows("ab", new[] { 2 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "ab" }));
        }

        [Test]
        public void Rows_reads_1_1_cells()
        {
            var rows = new Rows("a b", new[] { 1, 1 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "a", "b" }));
        }

        [Test]
        public void Rows_reads_2_4_cells()
        {
            var rows = new Rows("aa abcd", new[] { 2, 4 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "aa", "abcd" }));
        }

        [Test]
        public void Rows_reads_2_4_1_cells()
        {
            var rows = new Rows("aa abcd e", new[] { 2, 4, 1 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "aa", "abcd", "e" }));
        }

        [Test]
        public void Rows_reads_a_2_cell_but_with_1_character()
        {
            var rows = new Rows("a", new[] { 2 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "a" }));
        }

        [Test]
        public void Rows_reads_3_5_cell_but_with_2_3_character()
        {
            var rows = new Rows("id  sex  ", new[] { 3, 5 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "id", "sex" }));
        }

        [Test]
        public void Rows_reads_3_5_cell_but_with_2_3_character_()
        {
            var rows = new Rows("id    namn       tfn       ", new[] { 5, 10, 10 });
            Assert.That(rows.GetCells(), Is.EquivalentTo(new[] { "id", "namn", "tfn" }));
        }

        // GetCell

        [Test]
        public void GetCell_read_character_a()
        {
            var rows = new Rows("a", new[] { 1 });
            Assert.That(rows.GetCell(0, 1), Is.EqualTo("a"));
        }

        [Test]
        public void GetCell_read_character_b()
        {
            var rows = new Rows("b", new[] { 1 });
            Assert.That(rows.GetCell(0, 1), Is.EqualTo("b"));
        }

        [Test]
        public void GetCell_read_character_a_from_abc()
        {
            var rows = new Rows("abc", new[] { 3 });
            Assert.That(rows.GetCell(0, 1), Is.EqualTo("a"));
        }

        [Test]
        public void GetCell_read_character_ab_from_abc()
        {
            var rows = new Rows("abc", new[] { 3 });
            Assert.That(rows.GetCell(0, 2), Is.EqualTo("ab"));
        }

        [Test]
        public void GetCell_read_character_bc_from_abc()
        {
            var rows = new Rows("abc", new[] { 3 });
            Assert.That(rows.GetCell(1, 2), Is.EqualTo("bc"));
        }

        [Test]
        public void GetCell_read_2_character_from_a()
        {
            var rows = new Rows("a", new[] { 1 });
            Assert.That(rows.GetCell(0, 2), Is.EqualTo("a"));
        }

        [Test]
        public void GetCell_read_5_character_from_ab()
        {
            var rows = new Rows("ab", new[] { 2 });
            Assert.That(rows.GetCell(0, 5), Is.EqualTo("ab"));
        }

        [Test]
        public void GetCell_read_5_character_from_ab_cd()
        {
            var rows = new Rows("ab cd", new[] { 2, 5 });
            Assert.That(rows.GetCell(3, 5), Is.EqualTo("cd"));
        }

        [Test]
        public void GetCell_removed_trailing_spaces()
        {
            var rows = new Rows("ab   ", new[] { 5 });
            Assert.That(rows.GetCell(0, 5), Is.EqualTo("ab"));
        }
    }
}
