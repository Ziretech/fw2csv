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
    public class ByteBufferSpec
    {
        private readonly Encoding _encoding = Encoding.UTF8;


        private Stream CreateStream(string message)
        {
            var stream = new MemoryStream();
            var buffer = _encoding.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        [Test]
        public void ByteBuffer_reads_a_character()
        {
            var stream = CreateStream("a");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 1), Is.EqualTo("a"));
        }

        [Test]
        public void ByteBuffer_reads_b_character()
        {
            var stream = CreateStream("b");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 1), Is.EqualTo("b"));
        }

        [Test]
        public void ByteBuffer_reads_10_characters_that_fills_buffer()
        {
            var stream = CreateStream("abcdefghij");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 10), Is.EqualTo("abcdefghij"));
        }

        [Test]
        public void ByteBuffer_reads_10_characters_that_fills_buffer_when_11_characters_are_available()
        {
            var stream = CreateStream("abcdefghijk");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 10), Is.EqualTo("abcdefghij"));
        }

        [Test]
        public void ByteBuffer_returns_part_of_buffer()
        {
            var stream = CreateStream("abcdefghij");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 5), Is.EqualTo("abcde"));
        }

        [Test]
        public void ByteBuffer_throws_error_when_trying_to_read_more_characters_than_available()
        {
            var stream = CreateStream("abc");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            try
            {
                buffer.GetString(_encoding, 4);
                Assert.Fail("No exception was thrown.");
            }
            catch (ArgumentException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("exceeding number of characters"));
                Assert.That(exception.Message.ToLower(), Does.Contain("4"));
                Assert.That(exception.Message.ToLower(), Does.Contain("3"));
            }
        }

        [Test]
        public void ByteBuffer_returns_second_part_of_buffer()
        {
            var stream = CreateStream("abcdefghij");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            Assert.That(buffer.GetString(_encoding, 5), Is.EqualTo("fghij"));
        }

        [Test]
        public void ByteBuffer_returns_third_part_of_buffer()
        {
            var stream = CreateStream("abcdefghij");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            buffer.MoveBytesLeft(2);
            Assert.That(buffer.GetString(_encoding, 2), Is.EqualTo("hi"));
        }

        [Test]
        public void ByteBuffer_finds_end_of_buffer_when_it_is_one_character()
        {
            var stream = CreateStream("a");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(1));
        }

        [Test]
        public void ByteBuffer_finds_end_of_buffer_when_it_is_2_characters()
        {
            var stream = CreateStream("ab");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(2));
        }

        [Test]
        public void ByteBuffer_finds_end_of_buffer_when_it_is_9_chars_in_a_10_char_buffer()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(9));
        }

        [Test]
        public void ByteBuffer_finds_end_of_line_by_new_line()
        {
            var stream = CreateStream("abcde\nfghi");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(5));
        }

        [Test]
        public void ByteBuffer_finds_end_of_line_by_carriage_return()
        {
            var stream = CreateStream("abcd\r\nfghi");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(4));
        }

        [Test]
        public void ByteBuffer_finds_end_of_line_after_4_bytes()
        {
            var stream = CreateStream("a\nbcef\nghi");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(4), Is.EqualTo(6));
        }

        [Test]
        [Ignore("not sure this is resonable test (so how do we detect if buffer is to small?)")]
        public void ByteBuffer_throws_exception_when_no_end_of_buffer_found_in_buffer()
        {
            var stream = CreateStream("abcdefghij");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);

            try
            {
                buffer.NextLineLength(1);
                Assert.Fail("No exception was thrown.");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("no end of line"));
                Assert.That(exception.Message.ToLower(), Does.Contain("10"));
            }
            
        }
    }
}
