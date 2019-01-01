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

namespace Plexdata.Converters.Factories
{
    /// <summary>
    /// This static class provides the access to the creation of classes of types 
    /// <see cref="IBinConverter"/> and <see cref="IBinConverterSettings"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The idea behind this factory is to provide an abstraction between the 
    /// exposed interfaces and their default implementations.
    /// </para>
    /// <para>
    /// This factory might become unnecessary in case of using a different dependency 
    /// injection mechanism.
    /// </para>
    /// </remarks>
    public static class BinConverterFactory
    {
        /// <summary>
        /// This method creates an instance of <see cref="IBinConverterSettings"/> 
        /// and initializes it with its default values.
        /// </summary>
        /// <remarks>
        /// The instance of interface <see cref="IBinConverterSettings"/> returned 
        /// by this method will be initialized with almost always suitable settings.
        /// </remarks>
        /// <returns>
        /// An instance of <see cref="IBinConverterSettings"/> that is initialized 
        /// with its default values.
        /// </returns>
        /// <seealso cref="CreateConverter()"/>
        /// <seealso cref="CreateConverter(IBinConverterSettings)"/>
        public static IBinConverterSettings CreateSettings()
        {
            return new BinConverterSettings();
        }

        /// <summary>
        /// This method creates an instance of <see cref="IBinConverter"/> using 
        /// default settings.
        /// </summary>
        /// <remarks>
        /// The instance of interface <see cref="IBinConverter"/> returned by this 
        /// method can be directly used for creating a propper binary dump.
        /// </remarks>
        /// <returns>
        /// An instance of <see cref="IBinConverter"/>.
        /// </returns>
        /// <seealso cref="CreateSettings()"/>
        /// <seealso cref="CreateConverter(IBinConverterSettings)"/>
        public static IBinConverter CreateConverter()
        {
            return CreateConverter(null);
        }

        /// <summary>
        /// This method creates an instance of <see cref="IBinConverter"/> using 
        /// provided settings.
        /// </summary>
        /// <remarks>
        /// Users may want to initialize the binary dump settings beforehand. But keep 
        /// in mind, it will always be possible to directly modify the settings of the 
        /// returned instance.
        /// </remarks>
        /// <param name="settings">
        /// An instance of <see cref="IBinConverterSettings"/> to be used as settings. 
        /// A value of <c>null</c> causes the usage of default settings.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IBinConverter"/>.
        /// </returns>
        /// <seealso cref="CreateSettings()"/>
        /// <seealso cref="CreateConverter()"/>
        public static IBinConverter CreateConverter(IBinConverterSettings settings)
        {
            if (settings == null)
            {
                settings = CreateSettings();
            }

            return new BinConverter(settings);
        }
    }
}
