using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    interface IIsDisposable : IDisposable
    {
        bool IsDisposed { get; set; }
    }
}
