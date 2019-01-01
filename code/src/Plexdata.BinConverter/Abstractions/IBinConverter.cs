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
    /// This interface exposes all properties and other functionality needed to execute 
    /// a binary conversion.
    /// </summary>
    /// <remarks>
    /// This binary conversion takes place using current settings.
    /// </remarks>
    public interface IBinConverter
    {
        /// <summary>
        /// Gets an instance of <see cref="IBinConverterSettings"/> currently used as settings.
        /// </summary>
        /// <value>
        /// The instance of a class that implements this interface.
        /// </value>
        /// <remarks>
        /// An instance of this interface can be created using method 
        /// <see cref="Plexdata.Converters.Factories.BinConverterFactory.CreateSettings()"/>.
        /// </remarks>
        IBinConverterSettings Settings { get; }

        /// <summary>
        /// This method converts provided <paramref name="buffer"/> into its representation 
        /// of the hexadecimal output.
        /// </summary>
        /// <remarks>
        /// The whole buffer is taken for conversion.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be converted into a hexadecimal output.
        /// </param>
        /// <returns>
        /// A string representing the hexadecimal output of provided buffer.
        /// </returns>
        /// <seealso cref="IBinConverter.Convert(Byte[], Int32)"/>
        String Convert(Byte[] buffer);

        /// <summary>
        /// This method converts provided <paramref name="buffer"/> into its representation 
        /// of the hexadecimal output but uses the provided <paramref name="length"/> only.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An empty string is returned either if the <paramref name="buffer"/> is <c>null</c>, 
        /// or if the buffer length is zero, or if the <paramref name="length"/> is zero.
        /// </para>
        /// <para>
        /// If the length value exceeds the length of the buffer, then the output is limited to 
        /// the buffer length.
        /// </para>
        /// <para>
        /// If the length value is less than the length of the buffer, then the output is limited 
        /// to the provided length.
        /// </para>
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be converted into a hexadecimal output.
        /// </param>
        /// <param name="length">
        /// The number of bytes that should be included in the output.
        /// </param>
        /// <returns>
        /// A string representing the hexadecimal output of provided buffer.
        /// </returns>
        /// <seealso cref="IBinConverter.Convert(Byte[])"/>
        String Convert(Byte[] buffer, Int32 length);
    }
}
