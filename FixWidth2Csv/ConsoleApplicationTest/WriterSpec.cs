using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication;
using NUnit.Framework;

namespace ConsoleApplicationTest
{
    [TestFixture]
    public class WriterSpec
    {
        [Test]
        public void Writer_writes_one_row()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream, new MockConverter(), Environment.NewLine);

            writer.WriteRow(new [] {"unconvertedCell"});

            stream.Flush();
            stream.Position = 0;
            Assert.That(Encoding.GetEncoding("ISO-8859-1").GetString(stream.GetBuffer(), 0, (int)stream.Length), Is.EqualTo("convertedRow" + Environment.NewLine));
        }

        [Test]
        public void Writer_writes_one_cell()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream, new MockConverter(), Environment.NewLine);

            writer.WriteRow(new[] { "unconvertedCell" });
            writer.WriteRow(new[] { "unconvertedCell" });

            stream.Flush();
            stream.Position = 0;
            Assert.That(Encoding.GetEncoding("ISO-8859-1").GetString(stream.GetBuffer(), 0, (int)stream.Length), Is.EqualTo("convertedRow" + Environment.NewLine + "convertedRow" + Environment.NewLine));
        }

        [Test]
        public void Writer_writes_one_row_with_custom_line_separator()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream, new MockConverter(), "#*#");

            writer.WriteRow(new[] { "unconvertedCell" });

            stream.Flush();
            stream.Position = 0;
            Assert.That(Encoding.GetEncoding("ISO-8859-1").GetString(stream.GetBuffer(), 0, (int)stream.Length), Is.EqualTo("convertedRow#*#"));
        }

        //[Test]
        //public void Writer_writes_one_header()
        //{
        //    var stream = new MemoryStream();
        //    var writer = new Writer(stream, new MockConverter());

        //    writer.WriteRow(new[] { "a" });

        //    stream.Flush();
        //    stream.Position = 0;
        //    Assert.That(Encoding.GetEncoding("ISO-8859-1").GetString(stream.GetBuffer(), 0, (int)stream.Length), Is.EqualTo("b" + Environment.NewLine));
        //}

        public class MockConverter : IConverter
        {
            public string ConvertRow(string[] cells)
            {
                return "convertedRow";
            }
        }
    }
}
