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

using Plexdata.Converters.Abstractions;
using Plexdata.Converters.Factories;
using System;

namespace Plexdata.BinConverter.Inspector
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine(StandardExample(CreateBuffer(130)));
            Console.WriteLine(TwoByteBlockExample(CreateBuffer(129)));
            Console.WriteLine(FullByteBlockExample(CreateBuffer(130)));
            Console.WriteLine(OnlyByteBlockExample(CreateBuffer(130)));
            Console.WriteLine(OnlyTextBlockExample(CreateBuffer(130)));
            Console.WriteLine(LongAddressExample(CreateBuffer(130)));
            Console.WriteLine(DisableAddressDelimiterExample(CreateBuffer(130)));
            Console.WriteLine(ChangeAddressDelimiterExample(CreateBuffer(130)));
            Console.WriteLine(ChangeControlCharacterExample(CreateBuffer(130)));
            Console.WriteLine(ChangePaddingCharactersExample(CreateBuffer(130)));
            Console.WriteLine(BufferLimitationExample(CreateBuffer(25342)));
            Console.WriteLine(LowerCasesExample(CreateBuffer(256)));
            Console.WriteLine(AddressSizeAdjustmentExample(CreateBuffer(300)));

            Console.Write("Hit any key to finish... ");
            Console.ReadKey();
            Console.Write(Environment.NewLine);
        }

        private static String StandardExample(Byte[] buffer)
        {
            return BinConverterFactory.CreateConverter().Convert(buffer);
        }

        private static String TwoByteBlockExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.ByteBlockCount = 8;
            settings.ByteBlockWidth = 2;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String FullByteBlockExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.ByteBlockCount = 1;
            settings.ByteBlockWidth = 16;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String OnlyByteBlockExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.IsShowTextBlock = false;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String OnlyTextBlockExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.IsShowByteBlock = false;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String LongAddressExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.AddressSize = 8;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String DisableAddressDelimiterExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.AddressDelimiterWidth = 0;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String ChangeAddressDelimiterExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.AddressDelimiterValue = '#';

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String ChangeControlCharacterExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.ControlCharacterValue = '~';

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String ChangePaddingCharactersExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.ByteBlockPadding = '~';
            settings.TextBlockPadding = '+';

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String BufferLimitationExample(Byte[] buffer)
        {
            return BinConverterFactory.CreateConverter().Convert(buffer, 42);
        }

        private static String LowerCasesExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.IsCapitalLetters = false;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static String AddressSizeAdjustmentExample(Byte[] buffer)
        {
            IBinConverterSettings settings = BinConverterFactory.CreateSettings();

            settings.AddressSize = 1;

            return BinConverterFactory.CreateConverter(settings).Convert(buffer);
        }

        private static Byte[] CreateBuffer(Int32 length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be greater than or equal to zero.", nameof(length));
            }

            Byte[] result = new Byte[length];

            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index] = (Byte)index;
            }

            return result;
        }
    }
}
