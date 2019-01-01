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

using System;

namespace Plexdata.Converters.Abstractions
{
    /// <summary>
    /// This interface exposes all properties and other functionality needed to configure 
    /// a binary conversion.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The binary conversion settings are intended to allow users to adjust their binary 
    /// dumps to their own needs.
    /// </para>
    /// <para>
    /// The default implementation of this interface initializes all properties with its 
    /// default values.
    /// </para>
    /// </remarks>
    public interface IBinConverterSettings
    {
        /// <summary>
        /// Determines whether capital letters should be used for any hexadecimal value. 
        /// The default value is true.
        /// </summary>
        /// <value>
        /// True to use capital letters in every hexadecimal string and false to use lower 
        /// case letter instead.
        /// </value>
        /// <remarks>
        /// The value of this property actually affects addresses and the binary body part.
        /// </remarks>
        Boolean IsCapitalLetters { get; set; }

        /// <summary>
        /// Determines whether the address section should be visible or not. The default 
        /// value is true.
        /// </summary>
        /// <value>
        /// True enables the usage of the address section and false hides it.
        /// </value>
        /// <remarks>
        /// The value of this property also affects the visibility of the address delimiter.
        /// </remarks>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterValue"/>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterWidth"/>
        Boolean IsShowAddress { get; set; }

        /// <summary>
        /// Gets or sets the address size to be used. The default value is an address size 
        /// of 32 bits.
        /// </summary>
        /// <value>
        /// Allowed is one of the values 1, 2, 4 or 8.
        /// </value>
        /// <remarks>
        /// Keep in mind, the address size is more or less intended as some kind of suggestion. 
        /// The other way round, the actual used address size depends on the size of the buffer 
        /// as well.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is not one of the supported address sizes.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.IsShowAddress"/>
        Int32 AddressSize { get; set; }

        /// <summary>
        /// Determines whether the hexadecimal section should be visible or not. The default 
        /// value is true.
        /// </summary>
        /// <value>
        /// True enables the usage of the binary section and false hides it.
        /// </value>
        /// <remarks>
        /// If the value of this property is false then the section delimiter between body 
        /// and text is invisible as well.
        /// </remarks>
        Boolean IsShowByteBlock { get; set; }

        /// <summary>
        /// Gets or sets the number of hexadecimal blocks to be used per output line. The 
        /// default value is a count of 16 blocks.
        /// </summary>
        /// <value>
        /// The block count in combination with the block width represents the real line length.
        /// </value>
        /// <remarks>
        /// The value of this property must follow the calculation rule [2^n], with [n] is 
        /// [0, 1, 2, 3, ...]. Also keep in mind, the value of this property depends on the value 
        /// of property <see cref="ByteBlockWidth"/>. 
        /// </remarks>
        /// <example>
        /// See section Examples under <see cref="ByteBlockWidth"/> for some examples.
        /// </example>
        /// <exception cref="ArgumentException">
        /// This exception is throw eigher if provided value is less than or equal to zero, 
        /// or if provided value is NOT divisible by 8 bits.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.ByteBlockWidth"/>
        /// <seealso cref="IBinConverterSettings.ByteBlockLimit"/>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        Int32 ByteBlockCount { get; set; }

        /// <summary>
        /// Gets or sets the width of a single byte block within one line. The default value 
        /// is a width of one byte.
        /// </summary>
        /// <value>
        /// The block width in combination with the block count represents the real line length.
        /// </value>
        /// <remarks>
        /// The value of this property must follow the calculation rule [2^n], with [n] is 
        /// [0, 1, 2, 3, ...]. Also keep in mind, the value of this property depends on the value 
        /// of property <see cref="ByteBlockCount"/>. 
        /// </remarks>
        /// <example>
        /// <para>
        /// See table below for some examples of how the <see cref="ByteBlockCount"/> and the 
        /// <see cref="ByteBlockWidth"/> influencing each other.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Byte Block Count</term>
        /// <term>Byte Block Width</term>
        /// <term>Result</term>
        /// </listheader>
        /// <item>
        /// <description>16</description>
        /// <description>1</description>
        /// <description>00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F</description>
        /// </item>
        /// <item>
        /// <description>8</description>
        /// <description>2</description>
        /// <description>0001 0203 0405 0607 0809 0A0B 0C0D 0E0F</description>
        /// </item>
        /// <item>
        /// <description>4</description>
        /// <description>4</description>
        /// <description>00010203 04050607 08090A0B 0C0D0E0F</description>
        /// </item>
        /// <item>
        /// <description>2</description>
        /// <description>8</description>
        /// <description>0001020304050607 08090A0B0C0D0E0F</description>
        /// </item>
        /// <item>
        /// <description>1</description>
        /// <description>16</description>
        /// <description>000102030405060708090A0B0C0D0E0F</description>
        /// </item>
        /// </list>
        /// </example>
        /// <exception cref="ArgumentException">
        /// This exception is throw eigher if provided value is less than or equal to zero, 
        /// or if provided value is NOT divisible by 8 bits.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.ByteBlockCount"/>
        /// <seealso cref="IBinConverterSettings.ByteBlockLimit"/>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        Int32 ByteBlockWidth { get; set; }

        /// <summary>
        /// Gets the limit of the hexadecimal byte block section.
        /// </summary>
        /// <value>
        /// This property returns the real length of one single line in bytes.
        /// </value>
        /// <remarks>
        /// This convenient property actually represents the length of one line of the hexadecimal 
        /// block.
        /// </remarks>
        /// <seealso cref="IBinConverterSettings.ByteBlockCount"/>
        /// <seealso cref="IBinConverterSettings.ByteBlockWidth"/>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        Int32 ByteBlockLimit { get; }

        /// <summary>
        /// Gets or sets the character to be used as padding within the hexadecimal block. 
        /// The default value is an underscore.
        /// </summary>
        /// <value>
        /// The padding is taken to fill up still remaining binary block items. A space character 
        /// can be used. But note, the byte block padding does not mean the padding between two 
        /// hexadecimal block items. For this padding type the <see cref="SectionDelimiterValue"/> 
        /// is used instead.
        /// </value>
        /// <remarks>
        /// The value of this property only affects the last line of the hexadecimal block. 
        /// For example, if the last line of the hexadecimal block would contain empty items 
        /// until its regular line end, then the rest of this line will be padded like this: 
        /// <c>FF 42 23 05 29 __ __ __</c>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is one of the control characters.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.IsShowByteBlock"/>
        Char ByteBlockPadding { get; set; }

        /// <summary>
        /// Determines whether the textual section should be visible or not. The default 
        /// value is true.
        /// </summary>
        /// <value>
        /// True enables the usage of the text section and false hides it.
        /// </value>
        /// <remarks>
        /// The text representation might not be meaningful in every case. Therefore, disabling 
        /// it becomes possible by setting the value of this property to false.
        /// </remarks>
        Boolean IsShowTextBlock { get; set; }

        /// <summary>
        /// Gets or sets the character to be used as padding within the textual block. The 
        /// default value is an underscore.
        /// </summary>
        /// <value>
        /// The padding is taken to fill up still remaining text block items. A space character 
        /// can be used.
        /// </value>
        /// <remarks>
        /// The value of this property only affects the last line of the textual block. For 
        /// example, if the last line of the textual block would contain empty items until its 
        /// regular line end, then the rest of this line will be padded like this: <c>@v!.9___</c>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is one of the control characters.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.IsShowTextBlock"/>
        Char TextBlockPadding { get; set; }

        /// <summary>
        /// Gets or sets the character to be used as delimiter between the address block and 
        /// the rest of the output. The default value is a colon.
        /// </summary>
        /// <value>
        /// Set the value of property <see cref="AddressDelimiterWidth"/> to zero to disable 
        /// the usage of the address delimiter.
        /// </value>
        /// <remarks>
        /// The address delimiter is ignored if the address section is invisible.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is one of the control characters.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterWidth"/>
        Char AddressDelimiterValue { get; set; }

        /// <summary>
        /// Gets or sets the amount of taken address delimiters. The default value is one.
        /// </summary>
        /// <value>
        /// The address delimiter is repeated if the value of this property is greater than 
        /// one.
        /// </value>
        /// <remarks>
        /// A value of zero for this property does actually prevent the usage of the address 
        /// delimiter.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is less than zero.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.AddressDelimiterValue"/>
        Int32 AddressDelimiterWidth { get; set; }

        /// <summary>
        /// Gets or sets the character to be used as delimiter between each output section. 
        /// The default value is a space.
        /// </summary>
        /// <value>
        /// Set the value of property <see cref="SectionDelimiterWidth"/> to zero to disable 
        /// the usage of the section delimiter.
        /// </value>
        /// <remarks>
        /// The section delimiter is ignored if one of the sections is invisible. But keep in 
        /// mind, the value of this property is also used as separator between byte block items.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is one of the control characters.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterWidth"/>
        Char SectionDelimiterValue { get; set; }

        /// <summary>
        /// Gets or sets the amount of taken section delimiters. The default value is two.
        /// </summary>
        /// <value>
        /// The section delimiter is repeated if the value of this property is greater than 
        /// one.
        /// </value>
        /// <remarks>
        /// A value of zero for this property does actually prevent the usage of the section 
        /// delimiter.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is less than zero.
        /// </exception>
        /// <seealso cref="IBinConverterSettings.SectionDelimiterValue"/>
        Int32 SectionDelimiterWidth { get; set; }

        /// <summary>
        /// Gets or sets the character to be used as replacement of all control characters 
        /// inside the textual output block. The default value is a dot.
        /// </summary>
        /// <value>
        /// The control character is taken to replace any non-printable character. A space 
        /// character can be used.
        /// </value>
        /// <remarks>
        /// A subset of all possible characters is defined as control character. These are 
        /// for example the carriage return, the line feed, the tab-character, and many more. 
        /// Such control characters are usually not printable. Therefore, these characters 
        /// are usually replaced by printable character.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of provided value is one of the control characters.
        /// </exception>
        Char ControlCharacterValue { get; set; }
    }
}
