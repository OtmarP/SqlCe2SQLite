using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// OpSoftware.OpTools.UXHelper
// OpSoftware.OpLib
namespace OpSoftware.OpLib
{
    public static class UXHelper
    {
        public static bool CalcModulo(int iRow)
        {
            bool doEvents = false;

            // - 1,2,3,4,5,6,7,8,9,10
            // - 20,30,40,50,60,70,80,90,100
            // - 200,300,400,500,600,700,800,900,1000
            // - 2000,3000,4000,5000,6000,7000,8000,9000,10000
            // - 20000,30000,40000,50000,60000,70000, ...
            var iRowP1 = iRow + 1;
            if (iRowP1 <= 10)
            {
                doEvents = true;
            }
            else if (iRowP1 <= 100)
            {
                // mod 10
                if ((iRowP1 % 10) == 0) { doEvents = true; }
            }
            else if (iRowP1 <= 1000)
            {
                // mod 100
                if ((iRowP1 % 100) == 0) { doEvents = true; }
            }
            else if (iRowP1 <= 10000)
            {
                // mod 1000
                if ((iRowP1 % 1000) == 0) { doEvents = true; }
            }
            else if (iRowP1 <= 100000)
            {
                // mod 10000
                if ((iRowP1 % 10000) == 0) { doEvents = true; }
            }
            else if (iRowP1 <= 1000000)
            {
                // mod 100000
                if ((iRowP1 % 100000) == 0) { doEvents = true; }
            }
            else if (iRowP1 <= 10000000)
            {
                // mod 1000000
                if ((iRowP1 % 1000000) == 0) { doEvents = true; }
            }
            // ...

            return doEvents;
        }
    }
}
