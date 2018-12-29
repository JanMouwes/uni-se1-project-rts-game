using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.utils
{
    public static class ModulusUtils
    {
        /// <summary>
        /// Modulus that accounts for negative numbers.
        /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/remainder-operator">Here's for why this method exists.</see>
        /// </summary>
        /// <example>
        /// mod(22, 20) outputs 2
        /// because:
        /// 22 % 20 = 2,
        /// 2 + 20 = 22
        /// 22 % 20 = 2 
        /// </example>
        /// <example>
        /// mod(-18, 20) outputs 2
        /// because:
        /// -18 % 20 = -18,
        /// -18 + 20 = 2
        /// 2 % 20 = 2
        /// </example>
        /// <example>
        /// mod(-28, 20) outputs 12
        /// because:
        /// -28 % 20 = -8,
        /// -8 + 20 = 12
        /// 12 % 20 = 12
        /// </example>
        /// <param name="input"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public static int mod(int input, int modulus)
        {
            return (input % modulus + modulus) % modulus;
        }
    }
}