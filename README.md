## Plexdata Binary Converter

The _Plexdata Binary Converter_ represents a library that allows to create binary dumps. Main feature of this library is the possibility to configure the dump output in various ways. See below for some examples.

The software has been published under the terms of _MIT License_.

See the documentation under [https://akesseler.github.io/Plexdata.BinConverter/](https://akesseler.github.io/Plexdata.BinConverter/) for an introduction.

### Examples

Assuming a method with the task to create a dump string from a byte array could use the _Plexdata Binary Converter_ to accomplish this job. Such a method could look like shown below.

```
private String StandardExample(Byte[] buffer)
{
    return BinConverterFactory.CreateConverter().Convert(buffer);
}
```

Using the default implementation with its default settings would create an output string like shown here.

```
00000000:  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................
00000010:  10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F  ................
00000020:  20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F   !"#$%&'()*+,-./
00000030:  30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F  0123456789:;<=>?
00000040:  40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F  @ABCDEFGHIJKLMNO
00000050:  50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F  PQRSTUVWXYZ[\]^_
00000060:  60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F  `abcdefghijklmno
00000070:  70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F  pqrstuvwxyz{|}~.
00000080:  80 81 __ __ __ __ __ __ __ __ __ __ __ __ __ __  ..______________
```

With a little configuration it would be possible to group the binary section in blocks. If for example a grouping of two-byte-blocks is wanted than the above shown method would look like shown below.

```
private String TwoByteBlockExample(Byte[] buffer)
{
    IBinConverterSettings settings = BinConverterFactory.CreateSettings();

    settings.ByteBlockCount = 8;
    settings.ByteBlockWidth = 2;

    return BinConverterFactory.CreateConverter(settings).Convert(buffer);
}
```

The above code snippet create an output string that looks like shown below.

```
00000000:  0001 0203 0405 0607 0809 0A0B 0C0D 0E0F  ................
00000010:  1011 1213 1415 1617 1819 1A1B 1C1D 1E1F  ................
00000020:  2021 2223 2425 2627 2829 2A2B 2C2D 2E2F   !"#$%&'()*+,-./
00000030:  3031 3233 3435 3637 3839 3A3B 3C3D 3E3F  0123456789:;<=>?
00000040:  4041 4243 4445 4647 4849 4A4B 4C4D 4E4F  @ABCDEFGHIJKLMNO
00000050:  5051 5253 5455 5657 5859 5A5B 5C5D 5E5F  PQRSTUVWXYZ[\]^_
00000060:  6061 6263 6465 6667 6869 6A6B 6C6D 6E6F  `abcdefghijklmno
00000070:  7071 7273 7475 7677 7879 7A7B 7C7D 7E7F  pqrstuvwxyz{|}~.
00000080:  80__ ____ ____ ____ ____ ____ ____ ____  ._______________
```

More examples can be found in the above mentioned documentation.
