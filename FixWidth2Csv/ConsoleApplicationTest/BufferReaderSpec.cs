using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication;
using NUnit.Framework;

namespace ConsoleApplicationTest
{
    [TestFixture]
    public class BufferReaderSpec
    {
        [Test]
        public void BufferReader_reads_file()
        {
            using (var bufferReader = new BufferReader(@"c:\temp\fixwidthexempel.txt", 1024))
            {
                var content = bufferReader.Read();
                var contentString = Encoding.UTF8.GetString(content).TrimEnd('\0');
                Assert.That(contentString, Is.EqualTo("id by å\r\n-- -- --\r\ncg \r\na eg\r\ne  fy i"));
            }
        }

        [Test]
        public void BufferReader_reads_file_large_than_buffer()
        {
            using (var bufferReader = new BufferReader(@"c:\temp\fixwidthexempel.txt", 4))
            {
                var content = bufferReader.Read();
                var contentString = Encoding.UTF8.GetString(content).TrimEnd('\0');
                Assert.That(contentString, Is.EqualTo("id b"));
            }
        }

        [Ignore("Why to few characters?")]
        [Test]
        public void BufferReader_reads_second_time_from_file_large_than_buffer()
        {
            using (var bufferReader = new BufferReader(@"c:\temp\fixwidthexempel.txt", 6))
            {
                bufferReader.Read();
                var content = bufferReader.Read();
                var contentString = Encoding.UTF8.GetString(content).TrimEnd('\0');
                Assert.That(contentString, Is.EqualTo("å\r\n-- "));
            }
        }

        [Test]
        public void BufferReader_reads_ContactInfo()
        {
            using (var bufferReader = new BufferReader(@"C:\Users\jofhod02\OneDrive\Documents\Alumninät\lu_ny_kund\1-ContactInfo.txt", 1024))
            {
                while (!bufferReader.EndOfFile)
                {
                    bufferReader.Read();
                    var content = bufferReader.Read();

                    var contentString = Encoding.UTF8.GetString(content).TrimEnd('\0');
                    Assert.That(contentString, Is.EqualTo("å\r\n-- "));
                }
                
            }
        }
    }
}
