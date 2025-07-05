using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

//typedef struct _MSEMPLOYEE
//{
//    UINT  employeeID;
//    short employedYear;
//    char* displayName; 
//    char* alias; 
//} MSEMPLOYEE, *PMSEMPLOYEE;
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public class GameObjectCpp
{
    private uint _employeeID;
    private short _employedYear;
    private string _displayName;
    private string _alias;

    public uint EmployeeID
    {
        get { return _employeeID; }
        set { _employeeID = value; }
    }

    public short EmployedYear
    {
        get { return _employedYear; }
        set { _employedYear = value; }
    }

    public string DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }

    public string Alias
    {
        get { return _alias; }
        set { _alias = value; }
    }
}
