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
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plexdata.Converters
{
    /// <summary>
    /// This class represents the default implementation of interface <see cref="IBinConverterSettings"/>.
    /// </summary>
    /// <remarks>
    /// The default constructor of this class initializes all properties with its default values.
    /// </remarks>
    public class BinConverterSettings : IBinConverterSettings
    {
        #region Private field section.

        /// <summary>
        /// This field holds the value of used address size.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Int32 addressSize = 0;

        /// <summary>
        /// This field holds the value of used byte block count.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Int32 byteBlockCount = 0;

        /// <summary>
        /// This field holds the value of used byte block width.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Int32 byteBlockWidth = 0;

        /// <summary>
        /// This field holds the value of used byte block padding.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Char byteBlockPadding = '\0';

        /// <summary>
        /// This field holds the value of used text block padding.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Char textBlockPadding = '\0';

        /// <summary>
        /// This field holds the value of used address delimiter.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Char addressDelimiterValue = '\0';

        /// <summary>
        /// This field holds the value of used address delimiter width.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Int32 addressDelimiterWidth = 0;

        /// <summary>
        /// This field holds the value of used section delimiter.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Char sectionDelimiterValue = '\0';

        /// <summary>
        /// This field holds the value of used section delimiter width.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Int32 sectionDelimiterWidth = 0;

        /// <summary>
        /// This field holds the value of used control character replacement.
        /// </summary>
        /// <remarks>
        /// The default value is initialized in the default constructor.
        /// </remarks>
        private Char controlCharacterValue = '\0';

        #endregion

        #region Construction.

        /// <summary>
        /// The default constructor initializes an instance of this class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All properties of the new instance are initialized with their default values.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// Using default settings would produce an output that looks like as shown 
        /// below.
        /// </para>
        /// <code language="none">
        /// 00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................
        /// 00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................
        /// 00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !"#$%&amp;'()*+,-./
        /// 00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;&lt;=&gt;?
        /// 00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO
        /// 00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\]^_
        /// 00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno
        /// 00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.
        /// 00000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________
        /// </code>
        /// </example>
        /// <seealso cref="Factories.BinConverterFactory.CreateSettings()"/>
        public BinConverterSettings()
        {
            this.IsCapitalLetters = true;

            this.IsShowAddress = true;
            this.AddressSize = sizeof(UInt32);

            this.IsShowByteBlock = true;
            this.ByteBlockCount = 16;
            this.ByteBlockWidth = 1;
            this.ByteBlockPadding = '_';

            this.IsShowTextBlock = true;
            this.TextBlockPadding = '_';

            this.AddressDelimiterValue = ':';
            this.AddressDelimiterWidth = 1;

            this.SectionDelimiterValue = ' ';
            this.SectionDelimiterWidth = 2;

            this.ControlCharacterValue = '.';
        }

        #endregion

        #region Public property implementation.

        /// <inheritdoc cref="IBinConverterSettings.IsCapitalLetters" />
        public Boolean IsCapitalLetters { get; set; }

        /// <inheritdoc cref="IBinConverterSettings.IsShowAddress" />
        public Boolean IsShowAddress { get; set; }

        /// <inheritdoc cref="IBinConverterSettings.AddressSize" />
        public Int32 AddressSize
        {
            get
            {
                return this.addressSize;
            }
            set
            {
                switch (value)
                {
                    case sizeof(Byte):
                    case sizeof(UInt16):
                    case sizeof(UInt32):
                    case sizeof(UInt64):
                        this.addressSize = value;
                        break;
                    default:
                        throw new ArgumentException($"A value of {value} is not supported as address size.", nameof(this.AddressSize));
                }
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.IsShowByteBlock" />
        public Boolean IsShowByteBlock { get; set; }

        /// <inheritdoc cref="IBinConverterSettings.ByteBlockCount" />
        public Int32 ByteBlockCount
        {
            get
            {
                return this.byteBlockCount;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException($"The value of byte block count must be greater than zero.", nameof(this.ByteBlockCount));
                }

                if (!this.IsDivisibleByEightBits(value))
                {
                    throw new ArgumentException($"The byte block count of {value} is not divisible by 8 bits.", nameof(this.ByteBlockCount));
                }

                this.byteBlockCount = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.ByteBlockWidth" />
        public Int32 ByteBlockWidth
        {
            get
            {
                return this.byteBlockWidth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException($"The value of byte block width must be greater than zero.", nameof(this.ByteBlockWidth));
                }

                if (!this.IsDivisibleByEightBits(value))
                {
                    throw new ArgumentException($"The byte block width of {value} is not divisible by 8 bits.", nameof(this.ByteBlockWidth));
                }

                this.byteBlockWidth = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.ByteBlockLimit" />
        public Int32 ByteBlockLimit
        {
            get
            {
                return this.ByteBlockCount * this.ByteBlockWidth;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.ByteBlockPadding" />
        public Char ByteBlockPadding
        {
            get
            {
                return this.byteBlockPadding;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new ArgumentException("The value of the byte block padding should not be a control character.", nameof(this.ByteBlockPadding));
                }

                this.byteBlockPadding = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.IsShowTextBlock" />
        public Boolean IsShowTextBlock { get; set; }

        /// <inheritdoc cref="IBinConverterSettings.TextBlockPadding" />
        public Char TextBlockPadding
        {
            get
            {
                return this.textBlockPadding;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new ArgumentException("The value of the byte block padding should not be a control character.", nameof(this.TextBlockPadding));
                }

                this.textBlockPadding = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.AddressDelimiterValue" />
        public Char AddressDelimiterValue
        {
            get
            {
                return this.addressDelimiterValue;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new ArgumentException("The value of the address delimiter should not be a control character.", nameof(this.AddressDelimiterValue));
                }

                this.addressDelimiterValue = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.AddressDelimiterWidth" />
        public Int32 AddressDelimiterWidth
        {
            get
            {
                return this.addressDelimiterWidth;
            }
            set
            {

                if (value < 0)
                {
                    throw new ArgumentException("The address delimiter width should be zero or greater than zero.", nameof(this.AddressDelimiterWidth));
                }

                this.addressDelimiterWidth = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.SectionDelimiterValue" />
        public Char SectionDelimiterValue
        {
            get
            {
                return this.sectionDelimiterValue;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new ArgumentException("The value of the section delimiter should not be a control character.", nameof(this.SectionDelimiterValue));
                }

                this.sectionDelimiterValue = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.SectionDelimiterWidth" />
        public Int32 SectionDelimiterWidth
        {
            get
            {
                return this.sectionDelimiterWidth;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The section delimiter width should be zero or greater than zero.", nameof(this.SectionDelimiterWidth));
                }

                this.sectionDelimiterWidth = value;
            }
        }

        /// <inheritdoc cref="IBinConverterSettings.ControlCharacterValue" />
        public Char ControlCharacterValue
        {
            get
            {
                return this.controlCharacterValue;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new ArgumentException("The value of the control character replacement should not be a control character.", nameof(this.ControlCharacterValue));
                }

                this.controlCharacterValue = value;
            }
        }

        #endregion

        #region Public method implementation.

        /// <inheritdoc cref="Object.ToString()" />
        /// <remarks>
        /// Be aware, calling this method may take a while because Reflection is used to 
        /// determine each property.
        /// </remarks>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(512);
            PropertyInfo[] properties = this.GetType().GetRuntimeProperties().ToArray();

            if (properties.Length > 0)
            {
                builder.Append($"{this.GetType().Name}: ");

                for (Int32 index = 0; index < properties.Length; index++)
                {
                    PropertyInfo property = properties[index];

                    builder.Append($"{property.Name}=\"{property.GetValue(this)}\"");

                    if (index + 1 < properties.Length) { builder.Append($", "); }
                }
            }
            else
            {
                builder.Append(base.ToString());
            }

            return builder.ToString();
        }

        #endregion

        #region Private method implementation.

        /// <summary>
        /// This method determines if provided value is a multiple of 8 bits.
        /// </summary>
        /// <remarks>
        /// This method calculates the exponent [n] of [2^n] and checks if the 
        /// result does not have any fraction.
        /// </remarks>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <returns>
        /// True, if provided value is divisible by eight and false otherwise.
        /// </returns>
        private Boolean IsDivisibleByEightBits(Int32 value)
        {
            // Get the exponent [n] of [2^n] and check if its integer part is 
            // equal to the exponent. If so, then the value is based on 8 bits.
            Double exponent = Math.Log(value, 2);
            return exponent == (UInt64)exponent;
        }

        #endregion
    }
}
