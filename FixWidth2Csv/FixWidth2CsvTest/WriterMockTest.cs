﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    [TestFixture]
    public class WriterMockTest
    {
        private WriterMock _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new WriterMock();
        }

        [Test]
        public void WriterMock_have_empty_writeList_when_nothing_is_written()
        {
            Assert.That(_mock.WriteList, Is.Empty);
        }

        [Test]
        public void WriterMock_have_one_written_line()
        {
            _mock.Write("hej");
            Assert.That(_mock.WriteList, Is.EquivalentTo(new List<string>() { "hej" }));
        }

        [Test]
        public void WriterMock_have_two_written_lines()
        {
            _mock.Write("hej");
            _mock.Write("hej2");
            Assert.That(_mock.WriteList, Is.EquivalentTo(new List<string>() { "hej", "hej2" }));
        }
    }
}