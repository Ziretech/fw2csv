using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication;

namespace ConsoleApplicationTest
{
    [TestFixture]
    public class ReaderSpec
    {
        private Stream CreateStream(string message)
        {
            var stream = new MemoryStream();
            var buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        [Test]
        public void Reader_reads_text_data()
        {
            var reader = new Reader(CreateStream("data"));
            Assert.That(reader.ReadLine(4), Is.EqualTo("data"));
        }

        [Test]
        public void Reader_reads_text_data2()
        {
            var reader = new Reader(CreateStream("data2"));
            Assert.That(reader.ReadLine(4), Is.EqualTo("data2"));
        }

        [Test]
        public void Reader_throws_exception_when_line_exceeds_buffer()
        {
            var reader = new Reader(CreateStream("data2"), 2);
            try
            {
                reader.ReadLine(2);
                Assert.Fail("No exception was thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("exceed"));
                Assert.That(exception.Message.ToLower(), Does.Contain("2"));
            }
        }

        [Test]
        public void Reader_reads_first_line_ignoring_second()
        {
            var reader = new Reader(CreateStream("data1\r\ndata2"));
            Assert.That(reader.ReadLine(1), Is.EqualTo("data1"));
        }

        [Test]
        public void Reader_reads_first_line_when_line_break_have_no_carriage_return()
        {
            var reader = new Reader(CreateStream("data1\ndata2"));
            Assert.That(reader.ReadLine(1), Is.EqualTo("data1"));
        }

        [Test]
        public void Reader_reads_line_containing_new_line()
        {
            var reader = new Reader(CreateStream("data1\n\ndata2"));
            Assert.That(reader.ReadLine(1), Is.EqualTo("data1"));
        }

        [Ignore("Implement ByteBuffer")]
        [Test]
        public void Reader_reads_two_lines_of_three()
        {
            var reader = new Reader(CreateStream("data1\ndata2\ndata3"));
            reader.ReadLine(1);
            Assert.That(reader.ReadLine(1), Is.EqualTo("data2"));
        }
        // read more times
        // \n in the line
        
        // eftersom bufferten måste vara längre än en rad, sträva alltid efter att fylla den (räkna ut differensen)
    }
}
