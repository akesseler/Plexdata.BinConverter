/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using System;

namespace Plexdata.Converters.Tests
{
    public class BinConverterSettingsTests
    {
        private BinConverterSettings settings = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new BinConverterSettings();
        }

        [Test]
        public void BinConverterSettings_ValidationOfDefaultValues_ResultIsDefaultSettings()
        {
            Assert.That(this.settings.IsCapitalLetters, Is.EqualTo(true));
            Assert.That(this.settings.IsShowAddress, Is.EqualTo(true));
            Assert.That(this.settings.AddressSize, Is.EqualTo(4));
            Assert.That(this.settings.IsShowByteBlock, Is.EqualTo(true));
            Assert.That(this.settings.ByteBlockCount, Is.EqualTo(16));
            Assert.That(this.settings.ByteBlockWidth, Is.EqualTo(1));
            Assert.That(this.settings.ByteBlockLimit, Is.EqualTo(16 * 1));
            Assert.That(this.settings.ByteBlockPadding, Is.EqualTo('_'));
            Assert.That(this.settings.IsShowTextBlock, Is.EqualTo(true));
            Assert.That(this.settings.TextBlockPadding, Is.EqualTo('_'));
            Assert.That(this.settings.AddressDelimiterValue, Is.EqualTo(':'));
            Assert.That(this.settings.AddressDelimiterWidth, Is.EqualTo(1));
            Assert.That(this.settings.SectionDelimiterValue, Is.EqualTo(' '));
            Assert.That(this.settings.SectionDelimiterWidth, Is.EqualTo(2));
            Assert.That(this.settings.ControlCharacterValue, Is.EqualTo('.'));
        }

        [Test]
        public void AddressSize_CallingPropertySetterWithInvalidValue_ThrowsArgumentException()
        {
            Assert.That(() => this.settings.AddressSize = Int32.MaxValue, Throws.ArgumentException);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        [TestCase(8, 8)]
        public void AddressSize_CallingPropertySetterWithValidValue_ResultIsExpected(Int32 actual, Int32 expected)
        {
            this.settings.AddressSize = actual;
            Assert.That(this.settings.AddressSize, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        public void ByteBlockCount_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Int32 value)
        {
            Assert.That(() => this.settings.ByteBlockCount = value, Throws.ArgumentException);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        [TestCase(8, 8)]
        [TestCase(16, 16)]
        [TestCase(32, 32)]
        [TestCase(64, 64)]
        [TestCase(128, 128)]
        public void ByteBlockCount_CallingPropertySetterWithValidValue_ResultIsExpected(Int32 actual, Int32 expected)
        {
            this.settings.ByteBlockCount = actual;
            Assert.That(this.settings.ByteBlockCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        public void ByteBlockWidth_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Int32 value)
        {
            Assert.That(() => this.settings.ByteBlockWidth = value, Throws.ArgumentException);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        [TestCase(8, 8)]
        [TestCase(16, 16)]
        [TestCase(32, 32)]
        [TestCase(64, 64)]
        [TestCase(128, 128)]
        public void ByteBlockWidth_CallingPropertySetterWithValidValue_ResultIsExpected(Int32 actual, Int32 expected)
        {
            this.settings.ByteBlockWidth = actual;
            Assert.That(this.settings.ByteBlockWidth, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(InvalidControlCharacterItems))]
        public void ByteBlockPadding_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            Assert.That(() => this.settings.ByteBlockPadding = (Char)testData.Value, Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(ValidControlCharacterItems))]
        public void ByteBlockPadding_CallingPropertySetterWithValidValue_ResultIsExpected(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            this.settings.ByteBlockPadding = (Char)testData.Value;
            Assert.That(this.settings.ByteBlockPadding.ToString(), Is.EqualTo(testData.Result));
        }

        [Test]
        [TestCaseSource(nameof(InvalidControlCharacterItems))]
        public void TextBlockPadding_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            Assert.That(() => this.settings.TextBlockPadding = (Char)testData.Value, Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(ValidControlCharacterItems))]
        public void TextBlockPadding_CallingPropertySetterWithValidValue_ResultIsExpected(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            this.settings.TextBlockPadding = (Char)testData.Value;
            Assert.That(this.settings.TextBlockPadding.ToString(), Is.EqualTo(testData.Result));
        }

        [Test]
        [TestCaseSource(nameof(InvalidControlCharacterItems))]
        public void AddressDelimiterValue_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            Assert.That(() => this.settings.AddressDelimiterValue = (Char)testData.Value, Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(ValidControlCharacterItems))]
        public void AddressDelimiterValue_CallingPropertySetterWithValidValue_ResultIsExpected(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            this.settings.AddressDelimiterValue = (Char)testData.Value;
            Assert.That(this.settings.AddressDelimiterValue.ToString(), Is.EqualTo(testData.Result));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        public void AddressDelimiterWidth_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Int32 value)
        {
            Assert.That(() => this.settings.AddressDelimiterWidth = value, Throws.ArgumentException);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        [TestCase(8, 8)]
        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 11)]
        [TestCase(12, 12)]
        [TestCase(13, 13)]
        [TestCase(14, 14)]
        [TestCase(15, 15)]
        [TestCase(16, 16)]
        [TestCase(17, 17)]
        [TestCase(18, 18)]
        [TestCase(19, 19)]
        [TestCase(20, 20)]
        [TestCase(21, 21)]
        [TestCase(22, 22)]
        [TestCase(23, 23)]
        [TestCase(24, 24)]
        [TestCase(25, 25)]
        [TestCase(26, 26)]
        [TestCase(27, 27)]
        [TestCase(28, 28)]
        [TestCase(29, 29)]
        [TestCase(30, 30)]
        public void AddressDelimiterWidth_CallingPropertySetterWithValidValue_ResultIsExpected(Int32 actual, Int32 expected)
        {
            this.settings.AddressDelimiterWidth = actual;
            Assert.That(this.settings.AddressDelimiterWidth, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(InvalidControlCharacterItems))]
        public void SectionDelimiterValue_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            Assert.That(() => this.settings.SectionDelimiterValue = (Char)testData.Value, Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(ValidControlCharacterItems))]
        public void SectionDelimiterValue_CallingPropertySetterWithValidValue_ResultIsExpected(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            this.settings.SectionDelimiterValue = (Char)testData.Value;
            Assert.That(this.settings.SectionDelimiterValue.ToString(), Is.EqualTo(testData.Result));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        public void SectionDelimiterWidth_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Int32 value)
        {
            Assert.That(() => this.settings.SectionDelimiterWidth = value, Throws.ArgumentException);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        [TestCase(8, 8)]
        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 11)]
        [TestCase(12, 12)]
        [TestCase(13, 13)]
        [TestCase(14, 14)]
        [TestCase(15, 15)]
        [TestCase(16, 16)]
        [TestCase(17, 17)]
        [TestCase(18, 18)]
        [TestCase(19, 19)]
        [TestCase(20, 20)]
        [TestCase(21, 21)]
        [TestCase(22, 22)]
        [TestCase(23, 23)]
        [TestCase(24, 24)]
        [TestCase(25, 25)]
        [TestCase(26, 26)]
        [TestCase(27, 27)]
        [TestCase(28, 28)]
        [TestCase(29, 29)]
        [TestCase(30, 30)]
        public void SectionDelimiterWidth_CallingPropertySetterWithValidValue_ResultIsExpected(Int32 actual, Int32 expected)
        {
            this.settings.SectionDelimiterWidth = actual;
            Assert.That(this.settings.SectionDelimiterWidth, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(InvalidControlCharacterItems))]
        public void ControlCharacterValue_CallingPropertySetterWithInvalidValue_ThrowsArgumentException(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            Assert.That(() => this.settings.ControlCharacterValue = (Char)testData.Value, Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(ValidControlCharacterItems))]
        public void ControlCharacterValue_CallingPropertySetterWithValidValue_ResultIsExpected(Object testObject)
        {
            TestCharacter testData = (TestCharacter)testObject;
            this.settings.ControlCharacterValue = (Char)testData.Value;
            Assert.That(this.settings.ControlCharacterValue.ToString(), Is.EqualTo(testData.Result));
        }

        #region Private test data section...

        private class TestCharacter
        {
            private readonly Boolean showResult = false;

            public TestCharacter(Byte value)
            {
                this.Value = value;
            }

            public TestCharacter(Byte value, String result)
            {
                this.Value = value;
                this.Result = result;
                this.showResult = true;
            }

            public Byte Value { get; set; }

            public String Result { get; set; }

            public override String ToString()
            {
                String value = this.Value.ToString("X2");

                if (showResult)
                {
                    String result = this.Result;

                    if (this.Result == null)
                    {
                        result = "<null>";
                    }
                    else if (String.IsNullOrEmpty(this.Result))
                    {
                        result = "<empty>";
                    }
                    else if (String.IsNullOrWhiteSpace(this.Result))
                    {
                        result = "<whitespace>";
                    }

                    return $"0x{value}, \"{result}\"";
                }
                else
                {
                    return $"0x{value}";
                }
            }
        }

        private static readonly Object[] InvalidControlCharacterItems = new TestCharacter[] {
            new TestCharacter(0x00), new TestCharacter(0x01), new TestCharacter(0x02), new TestCharacter(0x03), new TestCharacter(0x04), new TestCharacter(0x05),
            new TestCharacter(0x06), new TestCharacter(0x07), new TestCharacter(0x08), new TestCharacter(0x09), new TestCharacter(0x0A), new TestCharacter(0x0B),
            new TestCharacter(0x0C), new TestCharacter(0x0D), new TestCharacter(0x0E), new TestCharacter(0x0F), new TestCharacter(0x10), new TestCharacter(0x11),
            new TestCharacter(0x12), new TestCharacter(0x13), new TestCharacter(0x14), new TestCharacter(0x15), new TestCharacter(0x16), new TestCharacter(0x17),
            new TestCharacter(0x18), new TestCharacter(0x19), new TestCharacter(0x1A), new TestCharacter(0x1B), new TestCharacter(0x1C), new TestCharacter(0x1D),
            new TestCharacter(0x1E), new TestCharacter(0x1F) };

        private static readonly Object[] ValidControlCharacterItems = new TestCharacter[] {
            new TestCharacter(0x20, " "), new TestCharacter(0x21, "!"), new TestCharacter(0x22, "\""), new TestCharacter(0x23, "#"),
            new TestCharacter(0x24, "$"), new TestCharacter(0x25, "%"), new TestCharacter(0x26, "&"),  new TestCharacter(0x27, "'"),
            new TestCharacter(0x28, "("), new TestCharacter(0x29, ")"), new TestCharacter(0x2A, "*"),  new TestCharacter(0x2B, "+"),
            new TestCharacter(0x2C, ","), new TestCharacter(0x2D, "-"), new TestCharacter(0x2E, ".") };

        #endregion
    }
}
