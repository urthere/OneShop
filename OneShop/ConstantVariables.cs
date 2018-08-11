using System.Collections.Generic;

namespace OneShop
{
    /// <summary>
    /// 公共变量
    /// </summary>
    public static class ConstantVariables
    {
        /// <summary>
        /// 条码最大长度
        /// </summary>
        public static readonly int BarcodeMaxLength = 13;

        /// <summary>
        /// 条码最小长度
        /// </summary>
        public static readonly int BarcodeMinLength = 5;

        public static readonly List<int> BarcodeLengthSet = new List<int>() { 8, 13, 12, 5, 6, 15 };
    }
}
