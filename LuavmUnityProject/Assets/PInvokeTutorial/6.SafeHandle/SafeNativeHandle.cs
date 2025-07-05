using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class SafeNativeHandle : SafeHandle
{
    public SafeNativeHandle() : base(IntPtr.Zero, true) { }

    public override bool IsInvalid
    {
        get { return IsClosed || handle == IntPtr.Zero; }
    }
}
