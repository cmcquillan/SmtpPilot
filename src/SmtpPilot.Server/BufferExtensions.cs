using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmtpPilot.Server
{
    public static class BufferExtensions
    {
        public static void Clear(this Span<char> span)
        {
            unsafe
            {
                fixed (char* ptr = span)
                {
                    byte* bptr = (byte*)ptr;

                    Unsafe.InitBlock(bptr, (byte)'\0', (uint)(sizeof(char) * span.Length));
                }
            }
        }
    }
}
