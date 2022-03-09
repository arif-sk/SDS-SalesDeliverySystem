using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Common.Utils
{
    public static class NumberGenerator
    {

        public static string GenerateOrderNo(int totalExistingOrder, int numLength)
        {
            var totalOrderString = totalExistingOrder.ToString();
            var len = totalOrderString.Length;
            var zeroLength = numLength - len;
            var zeros = GenerateZeros(zeroLength);
            return zeros + totalOrderString;
        }
        public static string GenerateZeros(int zeroLength)
        {
            string precZero = "";
            while(zeroLength > 0)
            {
                precZero += "0";
                zeroLength--;
            }
            return precZero;
        }
    }
}
