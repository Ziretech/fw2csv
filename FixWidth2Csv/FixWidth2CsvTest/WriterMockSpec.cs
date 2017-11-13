using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class WriterMockSpec
    {
        [Test]
        public void WriterMock_write_one_cell_a()
        {
            var writer = new WriterMock();
            writer.WriteRow(new [] {"a"});
            Assert.That(writer.RowList[0][0], Is.EqualTo("a"));
        }

        [Test]
        public void WriterMock_write_one_cell_bcd()
        {
            var writer = new WriterMock();
            writer.WriteRow(new[] { "bcd" });
            Assert.That(writer.RowList[0][0], Is.EqualTo("bcd"));
        }

        [Test]
        public void WriterMock_write_two_cells_on_one_row()
        {
            var writer = new WriterMock();
            writer.WriteRow(new[] { "ab", "cd" });
            Assert.That(writer.RowList[0][0], Is.EqualTo("ab"));
            Assert.That(writer.RowList[0][1], Is.EqualTo("cd"));
        }

        [Test]
        public void WriterMock_write_two_rows()
        {
            var writer = new WriterMock();
            writer.WriteRow(new[] { "ab" });
            writer.WriteRow(new[] { "cd" });
            Assert.That(writer.RowList[0][0], Is.EqualTo("ab"));
            Assert.That(writer.RowList[1][0], Is.EqualTo("cd"));
        }
    }
}
