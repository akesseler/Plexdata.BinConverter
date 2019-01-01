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
using Plexdata.Converters.Abstractions;
using System;
using System.Reflection;
using System.Text;

namespace Plexdata.Converters.Tests
{
    public class BinConverterTests
    {
        private IBinConverter converter = null;

        [SetUp]
        public void Setup()
        {
            this.converter = new BinConverter(new BinConverterSettings());
        }

        [Test]
        public void BinConverter_SettingsIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new BinConverter(null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCase(true, 100, 3)]
        [TestCase(false, 100, 0)]
        public void AddAddress_VariousSettings_ResultAsExpected(Boolean showAddress, Int32 address, Int32 expected)
        {
            String addrFormat = "{0}";
            StringBuilder dataBuilder = new StringBuilder();
            this.converter.Settings.IsShowAddress = showAddress;
            this.InvokePrivateMethod("AddAddress", this.converter, new Object[] { addrFormat, dataBuilder, address });
            Assert.That(dataBuilder.Length, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(true, true, 0xFF, 2, 1)]
        [TestCase(true, false, 0xFF, 2, 0)]
        [TestCase(false, true, 0xFF, 0, 1)]
        [TestCase(false, false, 0xFF, 0, 0)]
        public void AddValue_VariousSettings_ResultAsExpected(Boolean showByteBlock, Boolean showTextBlock, Byte value, Int32 expectedByteBlock, Int32 expectedTextBlock)
        {
            String bodyFormat = "{0:X2}";
            StringBuilder bodyBuilder = new StringBuilder();
            String textFormat = "{0}";
            StringBuilder textBuilder = new StringBuilder();

            this.converter.Settings.IsShowByteBlock = showByteBlock;
            this.converter.Settings.IsShowTextBlock = showTextBlock;

            this.InvokePrivateMethod("AddValue", this.converter, new Object[] { bodyFormat, bodyBuilder, textFormat, textBuilder, value });

            Assert.That(bodyBuilder.Length, Is.EqualTo(expectedByteBlock));
            Assert.That(textBuilder.Length, Is.EqualTo(expectedTextBlock));
        }

        [Test]
        [TestCase(true, true, 2, 1)]
        [TestCase(true, false, 2, 0)]
        [TestCase(false, true, 0, 1)]
        [TestCase(false, false, 0, 0)]
        public void AddPadding_VariousSettings_ResultAsExpected(Boolean showByteBlock, Boolean showTextBlock, Int32 expectedByteBlock, Int32 expectedTextBlock)
        {
            StringBuilder bodyBuilder = new StringBuilder();
            StringBuilder textBuilder = new StringBuilder();

            this.converter.Settings.IsShowByteBlock = showByteBlock;
            this.converter.Settings.IsShowTextBlock = showTextBlock;

            this.InvokePrivateMethod("AddPadding", this.converter, new Object[] { bodyBuilder, textBuilder });

            Assert.That(bodyBuilder.Length, Is.EqualTo(expectedByteBlock));
            Assert.That(textBuilder.Length, Is.EqualTo(expectedTextBlock));
        }

        [Test]
        [TestCase(true, true, "XX XX XX", "XXX", 2, 8 + 3 + 2)]
        [TestCase(true, false, "XX XX XX", "XXX", 2, 8 + 0 + 0)]
        [TestCase(false, true, "XX XX XX", "XXX", 2, 0 + 3 + 0)]
        [TestCase(false, false, "XX XX XX", "XXX", 2, 0 + 0 + 0)]
        public void AddResult_VariousSettings_ResultAsExpected(Boolean showByteBlock, Boolean showTextBlock, String bodyValue, String textValue, Int32 delimiterWidth, Int32 expectedLength)
        {
            StringBuilder dataBuilder = new StringBuilder();
            StringBuilder bodyBuilder = new StringBuilder(bodyValue);
            StringBuilder textBuilder = new StringBuilder(textValue);

            this.converter.Settings.IsShowByteBlock = showByteBlock;
            this.converter.Settings.IsShowTextBlock = showTextBlock;
            this.converter.Settings.SectionDelimiterValue = '#';
            this.converter.Settings.SectionDelimiterWidth = delimiterWidth;

            this.InvokePrivateMethod("AddResult", this.converter, new Object[] { dataBuilder, bodyBuilder, textBuilder });

            Assert.That(dataBuilder.Length, Is.EqualTo(expectedLength));
        }

        [Test]
        [TestCaseSource(nameof(AddressFormatTestItems))]
        public void GetAddressFormat_VariousSettings_ResultAsExpected(Object testObject)
        {
            AddressFormatTest testData = (AddressFormatTest)testObject;

            this.converter.Settings.IsCapitalLetters = testData.IsCapitalLetters;
            this.converter.Settings.AddressSize = testData.AddressSize;
            this.converter.Settings.AddressDelimiterValue = testData.AddressDelimiterValue;
            this.converter.Settings.AddressDelimiterWidth = testData.AddressDelimiterWidth;
            this.converter.Settings.SectionDelimiterValue = testData.SectionDelimiterValue;
            this.converter.Settings.SectionDelimiterWidth = testData.SectionDelimiterWidth;

            String actual = (String)this.InvokePrivateMethod("GetAddressFormat", this.converter, new Object[] { testData.TotalRequiredSize });

            Assert.That(actual, Is.EqualTo(testData.ExpectedResult));
        }

        [Test]
        [TestCase(true, "{0:X2}")]
        [TestCase(false, "{0:x2}")]
        public void GetBodyFormat_IsCapitalLetters_ResultAsExpected(Boolean capital, String expected)
        {
            this.converter.Settings.IsCapitalLetters = capital;
            String actual = (String)this.InvokePrivateMethod("GetBodyFormat", this.converter, null);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("{0}")]
        public void GetTextFormat_IsCapitalLetters_ResultAsExpected(String expected)
        {
            String actual = (String)this.InvokePrivateMethod("GetTextFormat", this.converter, null);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(true, 'X')]
        [TestCase(false, 'x')]
        public void GetByteFormat_IsCapitalLetters_ResultAsExpected(Boolean capital, Char expected)
        {
            this.converter.Settings.IsCapitalLetters = capital;
            Char actual = (Char)this.InvokePrivateMethod("GetByteFormat", this.converter, null);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, 8, 0)]
        [TestCase(1, 8, 8)]
        [TestCase(2, 8, 8)]
        [TestCase(7, 8, 8)]
        [TestCase(8, 8, 8)]
        [TestCase(9, 8, 16)]
        [TestCase(15, 8, 16)]
        [TestCase(16, 8, 16)]
        [TestCase(17, 8, 24)]
        [TestCase(18, 8, 24)]
        [TestCase(14, 16, 16)]
        [TestCase(15, 16, 16)]
        [TestCase(16, 16, 16)]
        [TestCase(17, 16, 32)]
        [TestCase(18, 16, 32)]
        public void GetTotalSize_CountAndLimit_ResultAsExpected(Int32 count, Int32 limit, Int32 expected)
        {
            Int32 actual = (Int32)this.InvokePrivateMethod("GetTotalSize", this.converter, new Object[] { count, limit });
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 4)]
        [TestCase(4, 8)]
        [TestCase(8, 16)]
        public void GetAddrFormatExpectedSize_AddressSize_ResultAsExpected(Int32 address, Int32 expected)
        {
            this.converter.Settings.AddressSize = address;
            Int32 actual = (Int32)this.InvokePrivateMethod("GetAddrFormatExpectedSize", this.converter, null);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(-2, 2)]
        [TestCase(-1, 2)]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(254, 2)]
        [TestCase(255, 2)]
        [TestCase(256, 2)]
        [TestCase(257, 4)]
        [TestCase(258, 4)]
        [TestCase(259, 4)]
        [TestCase(65534, 4)]
        [TestCase(65535, 4)]
        [TestCase(65536, 4)]
        [TestCase(65537, 8)]
        [TestCase(65538, 8)]
        [TestCase(65539, 8)]
        // The rest can't be tested and we have to trust in the algorithm...
        public void GetAddrFormatRequiredSize_Total_ResultAsExpected(Int32 total, Int32 expected)
        {
            Int32 actual = (Int32)this.InvokePrivateMethod("GetAddrFormatRequiredSize", this.converter, new Object[] { total });
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_Standard_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________";

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_TwoByteBlock_ResultAsExpected()
        {
            String expected = "00000000:  0001 0203 0405 0607 0809 0A0B 0C0D 0E0F  ................\r\n" +
                              "00000010:  1011 1213 1415 1617 1819 1A1B 1C1D 1E1F  ................\r\n" +
                              "00000020:  2021 2223 2425 2627 2829 2A2B 2C2D 2E2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  3031 3233 3435 3637 3839 3A3B 3C3D 3E3F  0123456789:;<=>?\r\n" +
                              "00000040:  4041 4243 4445 4647 4849 4A4B 4C4D 4E4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  5051 5253 5455 5657 5859 5A5B 5C5D 5E5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  6061 6263 6465 6667 6869 6A6B 6C6D 6E6F  `abcdefghijklmno\r\n" +
                              "00000070:  7071 7273 7475 7677 7879 7A7B 7C7D 7E7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  80__ ____ ____ ____ ____ ____ ____ ____  ._______________";

            this.converter.Settings.ByteBlockCount = 8;
            this.converter.Settings.ByteBlockWidth = 2;

            Assert.That(this.converter.Convert(this.CreateBuffer(129)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_FullByteBlock_ResultAsExpected()
        {
            String expected = "00000000:  000102030405060708090A0B0C0D0E0F  ................\r\n" +
                              "00000010:  101112131415161718191A1B1C1D1E1F  ................\r\n" +
                              "00000020:  202122232425262728292A2B2C2D2E2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  303132333435363738393A3B3C3D3E3F  0123456789:;<=>?\r\n" +
                              "00000040:  404142434445464748494A4B4C4D4E4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  505152535455565758595A5B5C5D5E5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  606162636465666768696A6B6C6D6E6F  `abcdefghijklmno\r\n" +
                              "00000070:  707172737475767778797A7B7C7D7E7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  8081____________________________  ..______________";

            this.converter.Settings.ByteBlockCount = 1;
            this.converter.Settings.ByteBlockWidth = 16;

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_OnlyByteBlock_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F\r\n" +
                              "00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F\r\n" +
                              "00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F\r\n" +
                              "00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F\r\n" +
                              "00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F\r\n" +
                              "00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F\r\n" +
                              "00000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __";

            this.converter.Settings.IsShowTextBlock = false;

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_OnlyTextBlock_ResultAsExpected()
        {
            String expected = "00000000:  ................\r\n" +
                              "00000010:  ................\r\n" +
                              "00000020:   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  0123456789:;<=>?\r\n" +
                              "00000040:  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  `abcdefghijklmno\r\n" +
                              "00000070:  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  ..______________";

            this.converter.Settings.IsShowByteBlock = false;

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_LongAddress_ResultAsExpected()
        {
            String expected = "0000000000000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "0000000000000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "0000000000000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "0000000000000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "0000000000000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "0000000000000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "0000000000000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "0000000000000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "0000000000000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________";

            this.converter.Settings.AddressSize = 8;

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_DisableAddressDelimiter_ResultAsExpected()
        {
            String expected = "00000000  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "00000010  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "00000020  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "00000040  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "00000070  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________";

            this.converter.Settings.AddressDelimiterWidth = 0;

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_ChangeAddressDelimiter_ResultAsExpected()
        {
            String expected = "00000000#  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "00000010#  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "00000020#  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030#  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "00000040#  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050#  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060#  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "00000070#  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080#  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________";

            this.converter.Settings.AddressDelimiterValue = '#';

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_ChangeControlCharacter_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ~~~~~~~~~~~~~~~~\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ~~~~~~~~~~~~~~~~\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~~\r\n" +
                              "00000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ~~______________";

            this.converter.Settings.ControlCharacterValue = '~';

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_ChangePaddingCharacters_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  80 81 ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~ ~~  ..++++++++++++++";

            this.converter.Settings.ByteBlockPadding = '~';
            this.converter.Settings.TextBlockPadding = '+';

            Assert.That(this.converter.Convert(this.CreateBuffer(130)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_BufferLimitation_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 __ __ __ __ __ __   !\"#$%&'()______";

            Assert.That(this.converter.Convert(this.CreateBuffer(25342), 42), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_LowerCases_ResultAsExpected()
        {
            String expected = "00000000:  00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f  ................\r\n" +
                              "00000010:  10 11 12 13 14 15 16 17 18 19 1a 1b 1c 1d 1e 1f  ................\r\n" +
                              "00000020:  20 21 22 23 24 25 26 27 28 29 2a 2b 2c 2d 2e 2f   !\"#$%&'()*+,-./\r\n" +
                              "00000030:  30 31 32 33 34 35 36 37 38 39 3a 3b 3c 3d 3e 3f  0123456789:;<=>?\r\n" +
                              "00000040:  40 41 42 43 44 45 46 47 48 49 4a 4b 4c 4d 4e 4f  @ABCDEFGHIJKLMNO\r\n" +
                              "00000050:  50 51 52 53 54 55 56 57 58 59 5a 5b 5c 5d 5e 5f  PQRSTUVWXYZ[\\]^_\r\n" +
                              "00000060:  60 61 62 63 64 65 66 67 68 69 6a 6b 6c 6d 6e 6f  `abcdefghijklmno\r\n" +
                              "00000070:  70 71 72 73 74 75 76 77 78 79 7a 7b 7c 7d 7e 7f  pqrstuvwxyz{|}~.\r\n" +
                              "00000080:  80 81 82 83 84 85 86 87 88 89 8a 8b 8c 8d 8e 8f  ................\r\n" +
                              "00000090:  90 91 92 93 94 95 96 97 98 99 9a 9b 9c 9d 9e 9f  ................\r\n" +
                              "000000a0:  a0 a1 a2 a3 a4 a5 a6 a7 a8 a9 aa ab ac ad ae af   ¡¢£¤¥¦§¨©ª«¬­®¯\r\n" +
                              "000000b0:  b0 b1 b2 b3 b4 b5 b6 b7 b8 b9 ba bb bc bd be bf  °±²³´µ¶·¸¹º»¼½¾¿\r\n" +
                              "000000c0:  c0 c1 c2 c3 c4 c5 c6 c7 c8 c9 ca cb cc cd ce cf  ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏ\r\n" +
                              "000000d0:  d0 d1 d2 d3 d4 d5 d6 d7 d8 d9 da db dc dd de df  ÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞß\r\n" +
                              "000000e0:  e0 e1 e2 e3 e4 e5 e6 e7 e8 e9 ea eb ec ed ee ef  àáâãäåæçèéêëìíîï\r\n" +
                              "000000f0:  f0 f1 f2 f3 f4 f5 f6 f7 f8 f9 fa fb fc fd fe ff  ðñòóôõö÷øùúûüýþÿ";

            this.converter.Settings.IsCapitalLetters = false;

            Assert.That(this.converter.Convert(this.CreateBuffer(256)), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_AddressSizeAdjustment_ResultAsExpected()
        {
            String expected = "0000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "0010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "0020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !\"#$%&'()*+,-./\r\n" +
                              "0030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?\r\n" +
                              "0040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO\r\n" +
                              "0050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\\]^_\r\n" +
                              "0060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno\r\n" +
                              "0070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.\r\n" +
                              "0080:  80 81 82 83 84 85 86 87 88 89 8A 8B 8C 8D 8E 8F  ................\r\n" +
                              "0090:  90 91 92 93 94 95 96 97 98 99 9A 9B 9C 9D 9E 9F  ................\r\n" +
                              "00A0:  A0 A1 A2 A3 A4 A5 A6 A7 A8 A9 AA AB AC AD AE AF   ¡¢£¤¥¦§¨©ª«¬­®¯\r\n" +
                              "00B0:  B0 B1 B2 B3 B4 B5 B6 B7 B8 B9 BA BB BC BD BE BF  °±²³´µ¶·¸¹º»¼½¾¿\r\n" +
                              "00C0:  C0 C1 C2 C3 C4 C5 C6 C7 C8 C9 CA CB CC CD CE CF  ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏ\r\n" +
                              "00D0:  D0 D1 D2 D3 D4 D5 D6 D7 D8 D9 DA DB DC DD DE DF  ÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞß\r\n" +
                              "00E0:  E0 E1 E2 E3 E4 E5 E6 E7 E8 E9 EA EB EC ED EE EF  àáâãäåæçèéêëìíîï\r\n" +
                              "00F0:  F0 F1 F2 F3 F4 F5 F6 F7 F8 F9 FA FB FC FD FE FF  ðñòóôõö÷øùúûüýþÿ\r\n" +
                              "0100:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................\r\n" +
                              "0110:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................\r\n" +
                              "0120:  20 21 22 23 24 25 26 27 28 29 2A 2B __ __ __ __   !\"#$%&'()*+____";

            this.converter.Settings.AddressSize = 1;

            Assert.That(this.converter.Convert(this.CreateBuffer(300)), Is.EqualTo(expected));
        }

        #region Private test data section...

        private class AddressFormatTest
        {
            public AddressFormatTest()
            {
                AddressDelimiterValue = '@';
                SectionDelimiterValue = '#';
            }

            public Boolean IsCapitalLetters { get; set; }
            public Int32 AddressSize { get; set; }
            public Char AddressDelimiterValue { get; set; }
            public Int32 AddressDelimiterWidth { get; set; }
            public Char SectionDelimiterValue { get; set; }
            public Int32 SectionDelimiterWidth { get; set; }
            public Int32 TotalRequiredSize { get; set; }
            public String ExpectedResult { get; set; }

            public override String ToString()
            {
                return $"{this.IsCapitalLetters}, " +
                       $"{this.AddressSize}, " +
                       $"\"{this.AddressDelimiterValue}\", " +
                       $"{this.AddressDelimiterWidth}, " +
                       $"\"{this.SectionDelimiterValue}\", " +
                       $"{this.SectionDelimiterWidth}, " +
                       $"{this.TotalRequiredSize}, " +
                       $"\"{this.ExpectedResult}\"";
            }
        }

        private static readonly Object[] AddressFormatTestItems = new AddressFormatTest[] {
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}##"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}##"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}##"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}##"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}##"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}##"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 0, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}@##"    },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}@##"    },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}@@##"    },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}@@##"    },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}@@##"    },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}@@##"    },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}@@##"    },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}@@##"    },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}@@##"   },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 2, SectionDelimiterWidth = 2, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}@@##"   },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}@"       },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}@"       },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}@"       },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}@"       },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}@"       },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}@"       },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}@"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 0, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}@"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}@#"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}@#"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}@#"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}@#"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}@#"      },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}@#"      },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}@#"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 1, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}@#"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:X2}@#####"  },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:x2}@#####"  },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:X4}@#####"  },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:x4}@#####"  },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:X8}@#####"  },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:x8}@#####"  },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:X16}@#####" },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 5, TotalRequiredSize = 10,       ExpectedResult = "{0:x16}@#####" },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:X8}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 1, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:x8}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:X8}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 2, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:x8}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:X8}@##"     },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 4, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:x8}@##"     },
            new AddressFormatTest { IsCapitalLetters = true,  AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:X16}@##"    },
            new AddressFormatTest { IsCapitalLetters = false, AddressSize = 8, AddressDelimiterWidth = 1, SectionDelimiterWidth = 2, TotalRequiredSize = 10000000, ExpectedResult = "{0:x16}@##"    },
        };

        private Object InvokePrivateMethod(String method, Object instance, Object[] parameters)
        {
            MethodInfo info = instance.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
            return info.Invoke(instance, parameters);
        }

        private Byte[] CreateBuffer(Int32 length)
        {
            Byte[] result = new Byte[length];

            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index] = (Byte)index;
            }

            return result;
        }

        #endregion
    }
}
