using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    public enum Length
    {
        Special = 0,
        Small = 1,
        Medium = 2,
        Long = 3,
    };

    public enum StateOfSluice
    {
        Up = 1,
        EnRoute = 2,
        Down = 3
    };
}
