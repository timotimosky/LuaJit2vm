using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

//typedef struct _SIMPLESTRUCT
//{
//    int    intValue;
//    short  shortValue;
//    float  floatValue;
//    double doubleValue;
//} SIMPLESTRUCT, *PSIMPLESTRUCT;
[StructLayout(LayoutKind.Sequential)]
public class ManagedClassBlittable
{
    private int _intValue;
    private short _shortValue;
    private float _floatValue;
    private double _doubleValue;

    public int IntValue
    {
        get { return _intValue; }
        set { _intValue = value; }
    }

    public short ShortValue
    {
        get { return _shortValue; }
        set { _shortValue = value; }
    }

    public float FloatValue
    {
        get { return _floatValue; }
        set { _floatValue = value; }
    }

    public double DoubleValue
    {
        get { return _doubleValue; }
        set { _doubleValue = value; }
    }
}
