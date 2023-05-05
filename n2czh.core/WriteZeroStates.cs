using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2czh.core
{
    [Flags]
    internal enum WriteZeroStates
    {
        None = 0b_0000_0000,
        BeenNonZero = 0b_0000_0001, //之前曾经为非零
        PreIsZero = 0b_0000_0010, //前位为零
        CurIsNonZero = 0b_0000_0100, //当前为非零
        Ready = BeenNonZero | PreIsZero | CurIsNonZero
    }
}
