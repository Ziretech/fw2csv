using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication;
using FixWidth2Csv;
using NUnit.Framework;
using System.IO;

namespace ConsoleApplicationTest
{
    [TestFixture]
    public class IntegrationTest_ConvertStream
    {
        [Test]
        public void Convert_stream()
        {
            var sourceString = "id  namn    ålder\r\n--- ------- -----\r\n1   klas    24\r\n4   ig\nor   30\r\n";
            var targetString = "id;namn;ålder\r\n1;klas;24\r\n4;ig\nor;30\r\n";

            var outputStream = new MemoryStream();
            var inputStream = new MemoryStream();
            var buffer = Encoding.UTF8.GetBytes(sourceString);
            inputStream.Write(buffer, 0, buffer.Length);
            inputStream.Seek(0, SeekOrigin.Begin);

            var writer = new Writer(outputStream, new CsvConverter(";"), "\r\n");
            var reader = new Reader(inputStream);
            var converter = new ConvertFixWidthToMatrix { Writer = writer };
            converter.Convert(reader);


            outputStream.Flush();
            outputStream.Position = 0;
            Assert.That(Encoding.GetEncoding("ISO-8859-1").GetString(outputStream.GetBuffer(), 0, (int)outputStream.Length), Is.EqualTo(targetString));
        }
    }
}
