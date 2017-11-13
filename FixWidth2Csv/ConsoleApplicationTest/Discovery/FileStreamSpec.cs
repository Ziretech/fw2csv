using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace ConsoleApplicationTest
{
    [TestFixture]
    public class FileStreamSpec
    {
        [Test]
        public void Read_characters_from_file()
        {
            var content = new StringBuilder();
            using (var stream = File.Open(@"c:\temp\fixwidthexempel.txt", FileMode.Open))
            {
                var buffer = new byte[1024];

                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    content.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                }
            }
            var contentString = content.ToString();
            Assert.That(contentString, Is.EqualTo("id by å\r\n-- -- --\r\ncg \r\na eg\r\ne  fy i"));
        }

        [Test]
        public void Read_line_from_file()
        {
            var content = new StringBuilder();
            string contentString;
            using (var stream = File.Open(@"c:\temp\fixwidthexempel.txt", FileMode.Open))
            {
                var streamReader = new StreamReader(stream);

                contentString = streamReader.ReadLine();
            }
            Assert.That(contentString, Is.EqualTo("id by å"));
        }

        [Test]
        public void Read_characters_after_line_from_file()
        {
            string content;
            using (var stream = File.Open(@"c:\temp\fixwidthexempel.txt", FileMode.Open))
            {
                var buffer = new byte[1024];
                var streamReader = new StreamReader(stream);

                streamReader.ReadLine();
                stream.Read(buffer, 0, buffer.Length);
                content = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            }

            Assert.That(content, Is.EqualTo("-- -- --\r\ncg \r\na eg\r\ne  fy i"));
        }

        [Test]
        public void Read_characters_from_stream_also_in_stream_reader()
        {
            string content;
            using (var stream = File.Open(@"c:\temp\fixwidthexempel.txt", FileMode.Open))
            {
                var buffer = new byte[1024];
                var streamReader = new StreamReader(stream);

                streamReader.ReadLine();
                stream.Read(buffer, 0, 4);
                content = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            }

            Assert.That(content, Is.EqualTo(""));
        }

        [Ignore("Uses too much memory")]
        [Test]
        public void Read_from_large_file()
        {
            var content = new StringBuilder();
            using (var stream = File.Open(@"C:\Users\jofhod02\OneDrive\Documents\Alumninät\lu_ny_kund\1-ContactInfo-002.txt", FileMode.Open))
            {
                var buffer = new byte[1024];

                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    content.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                }
            }
            var contentString = content.ToString();
            Assert.That(contentString.Length, Is.EqualTo(4));
        }
    }
}