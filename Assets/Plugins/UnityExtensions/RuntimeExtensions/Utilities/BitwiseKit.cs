
namespace UnityExtensions
{
    /// <summary>
    /// 位操作工具箱
    /// </summary>
    public struct BitwiseKit
    {
        /// <summary>
        /// 判断 value 是否包含 target 的所有非零二进制位
        /// </summary>
        public static bool Contain(int value, int target) { return (value & target) == target; }


        /// <summary>
        /// 判断两个值是否在某些二进制位都是非零的
        /// </summary>
        public static bool Intersect(int value1, int value2) { return (value1 & value2) != 0; }


        /// <summary>
        /// 从 value 中移除所有 target 的非零位
        /// </summary>
        public static int Remove(int value, int target) { return value & ~target; }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static sbyte SetBit0(sbyte value, int bit) { return (sbyte)(value & (~(1 << bit))); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static byte SetBit0(byte value, int bit) { return (byte)(value & (~(1u << bit))); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static short SetBit0(short value, int bit) { return (short)(value & (~(1 << bit))); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static ushort SetBit0(ushort value, int bit) { return (ushort)(value & (~(1u << bit))); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static int SetBit0(int value, int bit) { return value & (~(1 << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static uint SetBit0(uint value, int bit) { return value & (~(1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static long SetBit0(long value, int bit) { return value & (~(1L << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 0 后的值
        /// </summary>
        public static ulong SetBit0(ulong value, int bit) { return value & (~(1UL << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static sbyte SetBit1(sbyte value, int bit) { return (sbyte)((byte)value | (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static byte SetBit1(byte value, int bit) { return (byte)(value | (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static short SetBit1(short value, int bit) { return (short)((ushort)value | (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static ushort SetBit1(ushort value, int bit) { return (ushort)(value | (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static int SetBit1(int value, int bit) { return value | (1 << bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static uint SetBit1(uint value, int bit) { return value | (1u << bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static long SetBit1(long value, int bit) { return value | (1L << bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 后的值
        /// </summary>
        public static ulong SetBit1(ulong value, int bit) { return value | (1UL << bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static sbyte SetBit(sbyte value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static byte SetBit(byte value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static short SetBit(short value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static ushort SetBit(ushort value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static int SetBit(int value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static uint SetBit(uint value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static long SetBit(long value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位设置为 1 或 0 后的值
        /// </summary>
        public static ulong SetBit(ulong value, int bit, bool is1) { return is1 ? SetBit1(value, bit) : SetBit0(value, bit); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static sbyte ReverseBit(sbyte value, int bit) { return (sbyte)(value ^ (1 << bit)); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static byte ReverseBit(byte value, int bit) { return (byte)(value ^ (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static short ReverseBit(short value, int bit) { return (short)(value ^ (1 << bit)); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static ushort ReverseBit(ushort value, int bit) { return (ushort)(value ^ (1u << bit)); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static int ReverseBit(int value, int bit) { return value ^ (1 << bit); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static uint ReverseBit(uint value, int bit) { return value ^ (1u << bit); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static long ReverseBit(long value, int bit) { return value ^ (1L << bit); }


        /// <summary>
        /// 获得将指定二进制位反转后的值
        /// </summary>
        public static ulong ReverseBit(ulong value, int bit) { return value ^ (1UL << bit); }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(sbyte value, int bit) { return (value & (1 << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(byte value, int bit) { return (value & (1u << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(short value, int bit) { return (value & (1 << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(ushort value, int bit) { return (value & (1u << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(int value, int bit) { return (value & (1 << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(uint value, int bit) { return (value & (1u << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(long value, int bit) { return (value & (1L << bit)) != 0; }


        /// <summary>
        /// 获得指定二进制位的值
        /// </summary>
        public static bool GetBit(ulong value, int bit) { return (value & (1UL << bit)) != 0; }

    } // struct BitwiseKit

} // namespace UnityExtensions