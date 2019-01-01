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
using System;
using System.Text;

namespace Plexdata.Converters
{
    /// <summary>
    /// This class represents the default implementation of interface <see cref="IBinConverter"/>.
    /// </summary>
    /// <remarks>
    /// Executing a binary conversion takes place by using current settings. Users may change 
    /// these settings beforehand.
    /// </remarks>
    public class BinConverter : IBinConverter
    {
        #region Construction.

        /// <summary>
        /// This constructor initialize an instance of this class using provided settings.
        /// </summary>
        /// <remarks>
        /// An instance of interface <see cref="IBinConverterSettings"/> can be created using 
        /// the factory method <see cref="Factories.BinConverterFactory.CreateSettings()"/>.
        /// </remarks>
        /// <param name="settings">
        /// An instance of interface <see cref="IBinConverterSettings"/> to be used as default 
        /// settings.
        /// </param>
        /// <seealso cref="IBinConverter.Settings"/>
        /// <seealso cref="Factories.BinConverterFactory.CreateSettings()"/>
        /// <seealso cref="Factories.BinConverterFactory.CreateConverter()"/>
        /// <seealso cref="Factories.BinConverterFactory.CreateConverter(IBinConverterSettings)"/>
        public BinConverter(IBinConverterSettings settings)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion

        #region Public property implementation.

        /// <inheritdoc cref="IBinConverter.Settings" />
        public IBinConverterSettings Settings { get; private set; }

        #endregion

        #region Public method implementation.

        /// <inheritdoc cref="IBinConverter.Convert(Byte[])" />
        public String Convert(Byte[] buffer)
        {
            return this.Convert(buffer, -1);
        }

        /// <inheritdoc cref="IBinConverter.Convert(Byte[], Int32)" />
        public String Convert(Byte[] buffer, Int32 length)
        {
            if (buffer == null || buffer.Length == 0 || length == 0)
            {
                return String.Empty;
            }

            if (length < 0) { length = buffer.Length; }

            Int32 limit = this.Settings.ByteBlockLimit; // The actual length of one line...
            Int32 count = length > buffer.Length ? buffer.Length : length;
            Int32 total = this.GetTotalSize(count, limit);

            String addrFormat = this.GetAddressFormat(total);
            String bodyFormat = this.GetBodyFormat();
            String textFormat = this.GetTextFormat();

            StringBuilder dataBuilder = new StringBuilder(1024);
            StringBuilder bodyBuilder = new StringBuilder(128);
            StringBuilder textBuilder = new StringBuilder(128);

            for (Int32 index = 0; index < total; index++)
            {
                if (index % limit == 0)
                {
                    this.AddAddress(addrFormat, dataBuilder, index);
                }

                if (index < count)
                {
                    this.AddValue(bodyFormat, bodyBuilder, textFormat, textBuilder, buffer[index]);
                }
                else
                {
                    this.AddPadding(bodyBuilder, textBuilder);
                }

                if ((index + 1) % this.Settings.ByteBlockWidth == 0)
                {
                    bodyBuilder.Append(this.Settings.SectionDelimiterValue);
                }

                if ((index + 1) % limit == 0 || index + 1 >= total)
                {
                    this.AddResult(dataBuilder, bodyBuilder, textBuilder);

                    bodyBuilder.Length = 0;
                    textBuilder.Length = 0;
                }

                if ((index + 1) % limit == 0 && (index + 1) < total && dataBuilder.Length > 0)
                {
                    dataBuilder.Append(Environment.NewLine);
                }
            }

            return dataBuilder.ToString();
        }

        #endregion

        #region Private method implementation.

        /// <summary>
        /// This method adds an address to the result data using provided address format.
        /// </summary>
        /// <remarks>
        /// The address value is only added if showing the address block is enabled.
        /// </remarks>
        /// <param name="addrFormat">
        /// The address format to be used.
        /// </param>
        /// <param name="dataBuilder">
        /// The result data where to put the address into.
        /// </param>
        /// <param name="address">
        /// The address value to be added.
        /// </param>
        /// <seealso cref="IBinConverterSettings.IsShowAddress"/>
        /// <seealso cref="GetAddressFormat(Int32)" />
        private void AddAddress(String addrFormat, StringBuilder dataBuilder, Int32 address)
        {
            if (this.Settings.IsShowAddress)
            {
                dataBuilder.AppendFormat(addrFormat, address);
            }
        }

        /// <summary>
        /// This method adds a value to the temporary data buffers using provided format 
        /// data formats.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="value"/> is only added to the body if showing the body 
        /// block is enabled.
        /// </para>
        /// <para>
        /// The <paramref name="value"/> is only added to the text if showing the text 
        /// block is enabled.
        /// </para>
        /// </remarks>
        /// <param name="bodyFormat">
        /// The format to be used to add the value to the body section.
        /// </param>
        /// <param name="bodyBuilder">
        /// The temporary body buffer where to put the value into.
        /// </param>
        /// <param name="textFormat"></param>
        /// The format to be used to add the value to the text section.
        /// <param name="textBuilder">
        /// The temporary text buffer where to put the value into.
        /// </param>
        /// <param name="value">
        /// The value to be added.
        /// </param>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        /// <seealso cref="IBinConverterSettings.IsShowTextBlock"/>
        /// <seealso cref="GetBodyFormat()" />
        /// <seealso cref="GetTextFormat()" />
        private void AddValue(String bodyFormat, StringBuilder bodyBuilder, String textFormat, StringBuilder textBuilder, Byte value)
        {
            if (this.Settings.IsShowByteBlock)
            {
                bodyBuilder.AppendFormat(bodyFormat, value);
            }

            if (this.Settings.IsShowTextBlock)
            {
                textBuilder.AppendFormat(textFormat, Char.IsControl((Char)value) ? this.Settings.ControlCharacterValue : (Char)value);
            }
        }

        /// <summary>
        /// This method adds the padding to the temporary data buffers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The byte block padding always uses two of the defined character.
        /// </para>
        /// <para>
        /// The text block padding always uses one of the defined character.
        /// </para>
        /// </remarks>
        /// <param name="bodyBuilder">
        /// The temporary body buffer where to put the padding into.
        /// </param>
        /// <param name="textBuilder">
        /// The temporary text buffer where to put the padding into.
        /// </param>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        /// <seealso cref="IBinConverterSettings.IsShowTextBlock"/>
        /// <seealso cref="IBinConverterSettings.ByteBlockPadding"/>
        /// <seealso cref="IBinConverterSettings.TextBlockPadding"/>
        private void AddPadding(StringBuilder bodyBuilder, StringBuilder textBuilder)
        {
            if (this.Settings.IsShowByteBlock)
            {
                bodyBuilder.Append(String.Empty.PadLeft(2, this.Settings.ByteBlockPadding));
            }

            if (this.Settings.IsShowTextBlock)
            {
                textBuilder.Append(String.Empty.PadLeft(1, this.Settings.TextBlockPadding));
            }
        }

        /// <summary>
        /// The method adds the content of the temporary data buffers to the 
        /// result data buffer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The content of the temporary body block is only added to the result 
        /// data if showing the body block is enabled.
        /// </para>
        /// <para>
        /// The content of the temporary text block is only added to the result 
        /// data if showing the text block is enabled.
        /// </para>
        /// </remarks>
        /// <param name="dataBuilder">
        /// The result data where to put the temporary buffers into.
        /// </param>
        /// <param name="bodyBuilder">
        /// The temporary buffer with current body data.
        /// </param>
        /// <param name="textBuilder">
        /// The temporary buffer with current text data.
        /// </param>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        /// <seealso cref="IBinConverterSettings.IsShowTextBlock"/>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterValue"/>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterWidth"/>
        private void AddResult(StringBuilder dataBuilder, StringBuilder bodyBuilder, StringBuilder textBuilder)
        {
            if (this.Settings.IsShowByteBlock)
            {
                dataBuilder.Append(bodyBuilder.ToString().TrimEnd(this.Settings.SectionDelimiterValue));

                if (this.Settings.SectionDelimiterWidth > 0 && this.Settings.IsShowTextBlock)
                {
                    dataBuilder.Append($"{String.Empty.PadLeft(this.Settings.SectionDelimiterWidth, this.Settings.SectionDelimiterValue)}");
                }
            }

            if (this.Settings.IsShowTextBlock)
            {
                dataBuilder.Append(textBuilder.ToString());
            }
        }

        /// <summary>
        /// This method generates the address format to be used.
        /// </summary>
        /// <remarks>
        /// The actual generated address format depends on certain conditions 
        /// and settings. One of the conditions is for example the minimum 
        /// required address size. Another condition is for example whether 
        /// the address delimiter is used at all.
        /// </remarks>
        /// <param name="total">
        /// The total number of bytes to be used for the body section. This 
        /// value has to include possibly available padding bytes.
        /// </param>
        /// <returns>
        /// A string that defines how to format each of the address values.
        /// </returns>
        /// <seealso cref="GetByteFormat()"/>
        /// <seealso cref="GetAddrFormatExpectedSize()"/>
        /// <seealso cref="GetAddrFormatRequiredSize(Int32)"/>
        /// <seealso cref="GetTotalSize(Int32, Int32)"/>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterWidth"/>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterValue"/>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterWidth"/>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterValue"/>
        /// <seealso cref="IBinConverterSettings.IsCapitalLetters"/>
        private String GetAddressFormat(Int32 total)
        {
            Int32 expected = this.GetAddrFormatExpectedSize();
            Int32 required = this.GetAddrFormatRequiredSize(total);

            expected = required > expected ? required : expected;

            String padding = String.Empty;

            if (this.Settings.AddressDelimiterWidth > 0)
            {
                padding += $"{String.Empty.PadLeft(this.Settings.AddressDelimiterWidth, this.Settings.AddressDelimiterValue)}";
            }

            if (this.Settings.SectionDelimiterWidth > 0)
            {
                padding += $"{String.Empty.PadLeft(this.Settings.SectionDelimiterWidth, this.Settings.SectionDelimiterValue)}";
            }

            return $"{{0:{this.GetByteFormat()}{expected}}}{padding}";
        }

        /// <summary>
        /// This method builds the string to be used to format each byte of 
        /// the body section.
        /// </summary>
        /// <remarks>
        /// The result string always uses two characters per byte but its 
        /// hexadecimal character depends on current usage of capital letters.
        /// </remarks>
        /// <returns>
        /// A string to format body byte items.
        /// </returns>
        /// <seealso cref="GetByteFormat()"/>
        /// <seealso cref="IBinConverterSettings.IsCapitalLetters"/>
        private String GetBodyFormat()
        {
            return $"{{0:{this.GetByteFormat()}2}}";
        }

        /// <summary>
        /// This method builds the string to be used to format each character 
        /// of the text section.
        /// </summary>
        /// <remarks>
        /// This method always returns the same format string which formats 
        /// exactly one character.
        /// </remarks>
        /// <returns>
        /// A string to format text character items.
        /// </returns>
        private String GetTextFormat()
        {
            return $"{{0}}";
        }

        /// <summary>
        /// This method returns the character to be used for hexadecimal byte 
        /// formatting.
        /// </summary>
        /// <remarks>
        /// The result character depends on current usage of capital letters.
        /// </remarks>
        /// <returns>
        /// The character of how to format bytes as hexadecimal string.
        /// </returns>
        /// <seealso cref="IBinConverterSettings.IsCapitalLetters"/>
        private Char GetByteFormat()
        {
            return this.Settings.IsCapitalLetters ? 'X' : 'x';
        }

        /// <summary>
        /// This method determines the total number of bytes to be used for 
        /// the body section. 
        /// </summary>
        /// <remarks>
        /// The returned total number of bytes includes the padding bytes as 
        /// well and is therefore in most cases greater than the value of 
        /// <paramref name="count"/>.
        /// </remarks>
        /// <param name="count">
        /// This value represents the actual number of affected buffer bytes.
        /// </param>
        /// <param name="limit">
        /// This value represents the actual used line length.
        /// </param>
        /// <returns>
        /// The number of bytes to ensure a matrix of minimum required lines 
        /// and bytes per line.
        /// </returns>
        /// <seealso cref="IBinConverterSettings.ByteBlockLimit"/>
        private Int32 GetTotalSize(Int32 count, Int32 limit)
        {
            // Calculate minimum number of required lines.
            Int32 lines = count / limit;

            // Add one more line if necessary.
            if (count % limit != 0) { lines += 1; }

            // Calculate the total number of required 
            // bytes (including padding) and return it.
            return lines * limit;
        }

        /// <summary>
        /// This method determines the number of characters needed to format 
        /// an address.
        /// </summary>
        /// <remarks>
        /// The returned value is nothing else but the current address size 
        /// that is just multiplied by two.
        /// </remarks>
        /// <returns>
        /// The number of characters to format an address.
        /// </returns>
        /// <seealso cref="IBinConverterSettings.AddressSize"/>
        private Int32 GetAddrFormatExpectedSize()
        {
            return 2 * this.Settings.AddressSize;
        }

        /// <summary>
        /// This method determines the minimum address format size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The minimum required number of characters depends on the 
        /// total affected buffer size, which includes possibly available 
        /// padding bytes as well.
        /// </para>
        /// <para>
        /// In detail, this method check if the last possible address 
        /// fits into either one byte, or into two bytes, or into four 
        /// bytes or into eight bytes.
        /// </para>
        /// </remarks>
        /// <param name="total">
        /// The total number of bytes to be used for the body section. This 
        /// value has to include possibly available padding bytes.
        /// </param>
        /// <returns>
        /// The minimum number of required characters to be able to format 
        /// addresses.
        /// </returns>
        /// <seealso cref="GetAddressFormat(Int32)"/>
        private Int32 GetAddrFormatRequiredSize(Int32 total)
        {
            Int64 address = total - 1; // Determine last possible address from total length.

            if (address <= Byte.MaxValue)
            {
                return 2 * sizeof(Byte);
            }
            else if (address <= UInt16.MaxValue)
            {
                return 2 * sizeof(UInt16);
            }
            else if (address <= UInt32.MaxValue)
            {
                return 2 * sizeof(UInt32);
            }
            else
            {
                return 2 * sizeof(UInt64);
            }
        }

        #endregion
    }
}
