using System;
using System.IO;

namespace OneShop.Model
{
    public static class OneLog
    {
        public static void WriteLog(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy-MM-dd") + @".txt";
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(message);
                sw.Flush();
            }
        }
    }
}
