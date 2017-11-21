using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public void ByteBuffer_dont_remove_anything_from_buffer_not_beginning_with_end_of_line()
        {
            var stream = CreateStream("abcde");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            buffer.RemoveEndLine();
            Assert.That(buffer.GetString(_encoding, 5), Is.EqualTo("abcde"));
        }

        [Test]
        public void ByteBuffer_dont_remove_anything_from_empty_buffer()
        {
            var buffer = new ByteBuffer(10);
            buffer.RemoveEndLine();
        }

        [Test]
        public void ByteBuffer_remove_new_line_from_buffer_beginning_with_new_line()
        {
            var stream = CreateStream("\nabcde");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            buffer.RemoveEndLine();
            Assert.That(buffer.GetString(_encoding, 5), Is.EqualTo("abcde"));
        }

        [Test]
        public void ByteBuffer_remove_new_line_from_buffer_beginning_with_carriage_return_new_line()
        {
            var stream = CreateStream("\r\nabcde");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            buffer.RemoveEndLine();
            Assert.That(buffer.GetString(_encoding, 5), Is.EqualTo("abcde"));
        }

        [Test]
        public void ByteBuffer_fills_buffer_two_times()
        {
            var stream = CreateStream("abcdefghijk");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(4);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 4), Is.EqualTo("efgh"));
        }

        [Test]
        public void ByteBuffer_fills_buffer_two_times_with_no_removal_between()
        {
            var stream = CreateStream("abcdefghijk");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 4), Is.EqualTo("abcd"));
        }

        [Test]
        public void ByteBuffer_refills_buffer_and_reads_end()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.GetString(_encoding, 4), Is.EqualTo("fghi"));
        }

        [Test]
        public void ByteBuffer_refills_buffer_and_returns_number_of_bytes()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.NextLineLength(1), Is.EqualTo(4));
        }

        [Test]
        public void ByteBuffer_throws_exception_when_last_line_is_shorter_than_required()
        {
            var stream = CreateStream("abcde");
            var buffer = new ByteBuffer(10);
            buffer.FillBuffer(stream);
            try
            {
                buffer.NextLineLength(7);
                Assert.Fail("No exception was thrown.");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception.Message.ToLower(), Does.Contain("line is shorter"));
                Assert.That(exception.Message.ToLower(), Does.Contain("5"));
                Assert.That(exception.Message.ToLower(), Does.Contain("7"));
            }
            
        }

        [Test]
        public void ByteBuffer_with_no_data_is_empty()
        {
            var buffer = new ByteBuffer(5);
            Assert.That(buffer.IsEmpty, Is.True);
        }

        [Test]
        public void ByteBuffer_with_data_is_not_empty()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.IsEmpty, Is.False);
        }

        [Test]
        public void ByteBuffer_with_emptied_buffer_is_empty()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            Assert.That(buffer.IsEmpty, Is.True);
        }

        [Test]
        public void ByteBuffer_with_refilled_buffer_is_not_empty()
        {
            var stream = CreateStream("abcdefghi");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            buffer.MoveBytesLeft(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.IsEmpty, Is.False);
        }

        [Test]
        public void ByteBuffer_identifies_that_buffer_begins_with_a()
        {
            var stream = CreateStream("a");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.BeginWithSequence(new []{(byte)'a'}), Is.True);
        }

        [Test]
        public void ByteBuffer_identifies_that_buffer_dont_begin_with_a()
        {
            var stream = CreateStream("b");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.BeginWithSequence(new[] { (byte)'a' }), Is.False);
        }

        [Test]
        public void ByteBuffer_identifies_that_buffer_begins_with_ab()
        {
            var stream = CreateStream("ab");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.BeginWithSequence(new[] { (byte)'a', (byte)'b' }), Is.True);
        }

        [Test]
        public void ByteBuffer_identifies_that_buffer_with_ac_dont_begins_with_ab()
        {
            var stream = CreateStream("ac");
            var buffer = new ByteBuffer(5);
            buffer.FillBuffer(stream);
            Assert.That(buffer.BeginWithSequence(new[] { (byte)'a', (byte)'b' }), Is.False);
        }

        [Test]
        public void ByteBuffer_identifies_that_buffer_shorter_than_sequence_cant_contain_sequence()
        {
            var stream = CreateStream("datat");
            var buffer = new ByteBuffer(2);
            buffer.FillBuffer(stream);
            Assert.That(buffer.BeginWithSequence(new[] { (byte)'d', (byte)'a', (byte)'t' }), Is.False);
        }
    }
}
