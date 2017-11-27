using ConsoleAdapter;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace ConsoleAdapterTest
{
    [TestFixture]
    public class ReaderSpec
    {
        private Stream CreateStream(string message)
        {
            return CreateStream(Encoding.UTF8.GetBytes(message));
        }

        private Stream CreateStream(byte[] buffer)
        {
            var stream = new MemoryStream();
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
            Assert.That(reader.ReadLine(6), Is.EqualTo("data1\n"));
        }
        
        [Test]
        public void Reader_reads_two_lines_of_three()
        {
            var reader = new Reader(CreateStream("data1\ndata2\ndata3"));
            reader.ReadLine(1);
            Assert.That(reader.ReadLine(1), Is.EqualTo("data2"));
        }

        [Test]
        public void Reader_reads_new_line_to_next_new_line()
        {
            var reader = new Reader(CreateStream("data1\ndata2\ndata3"));
            Assert.That(reader.ReadLine(8), Is.EqualTo("data1\ndata2"));
        }

        [Test]
        public void Reader_reads_carriage_return_new_line_to_next_new_line()
        {
            var reader = new Reader(CreateStream("data1\r\ndata2\ndata3"));
            Assert.That(reader.ReadLine(8), Is.EqualTo("data1\r\ndata2"));
        }

        [Test]
        public void Reader_reads_new_line_to_next_carriage_return_new_line()
        {
            var reader = new Reader(CreateStream("data1\ndata2\r\ndata3"));
            Assert.That(reader.ReadLine(8), Is.EqualTo("data1\ndata2"));
        }

        [Test]
        public void Reader_removes_BOM_from_first_stream_entry()
        {
            var reader = new Reader(CreateStream(new[] { (byte)0xEF, (byte)0xBB, (byte)0xBF, (byte)'a' }));
            Assert.That(reader.ReadLine(0), Is.EqualTo("a"));
        }

        [Test]
        public void Reader_finds_no_lines_in_empty_stream()
        {
            var reader = new Reader(CreateStream(""));
            Assert.That(reader.MoreLines, Is.False);
        }

        [Test]
        public void Reader_finds_lines_in_stream_with_line()
        {
            var reader = new Reader(CreateStream("abcdef"));
            Assert.That(reader.MoreLines, Is.True);
        }

        [Test]
        public void Reader_finds_no_lines_after_all_is_read()
        {
            var reader = new Reader(CreateStream("abcdef"));
            reader.ReadLine(0);
            Assert.That(reader.MoreLines, Is.False);
        }

        [Test]
        public void Reader_finds_more_lines_after_first_is_read()
        {
            var reader = new Reader(CreateStream("id  namn\n--- ----\n1   åke\n2   anna"));
            reader.ReadLine(4);
            Assert.That(reader.MoreLines, Is.True);
        }

        [Test]
        public void Reader_finds_no_more_lines_after_all_is_read()
        {
            var reader = new Reader(CreateStream("id  namn\n--- ----\n1   åke\n2   anna"));
            reader.ReadLine(4);
            reader.ReadLine(4);
            reader.ReadLine(4);
            reader.ReadLine(4);
            Assert.That(reader.MoreLines, Is.False);
        }

        [Test]
        public void Reader_finds_no_more_lines_after_all_is_read_and_last_ending_with_new_line()
        {
            var reader = new Reader(CreateStream("id  namn\n--- ----\n1   åke\n2   anna\n"));
            reader.ReadLine(4);
            reader.ReadLine(4);
            reader.ReadLine(4);
            reader.ReadLine(4);
            Assert.That(reader.MoreLines, Is.False);
        }

        [Test]
        public void Reader_throws_exception_when_disallowed_string_a_is_found_in_line()
        {
            var reader = new Reader(CreateStream("a"), 10, new [] {"a"});
            try
            {
                reader.ReadLine(1);
                Assert.Fail("No exception is thrown.");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("disallowed characters"));
                Assert.That(exception.Message.ToLower(), Does.Contain("a"));
            }
        }
    }
}
