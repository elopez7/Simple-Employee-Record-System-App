using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary.Defaults
{
    public static class DefaultGlobalValues
    {
        public static decimal MinimumDollarAmount { get; private set; } = 1;
        public static decimal MaximumDollarAmount { get; private set; } = 30000;
        public static decimal DefaultStartingSalary { get; private set; } = 30000;
        public static int FirstEmployeeNumber { get; set; } = 1;
        public static int LastEmployeeNumber { get; set; } = 100;
    }
}
