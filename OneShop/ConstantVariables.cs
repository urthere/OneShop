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
        public static readonly int BARCODEMAXLENGTH = 13;

        /// <summary>
        /// 条码最小长度
        /// </summary>
        public static readonly int BARCODEMINLENGTH = 5;

        /// <summary>
        /// 原点 x
        /// </summary>
        public static readonly int XAXISORIGIN = 100;

        /// <summary>
        /// 原点 y
        /// </summary>
        public static readonly int YAXISORIGIN = 600;

        public enum DaysOfMonth
        {
            BigSmallMonth = 775,
            February = 756
        }
    }
}
