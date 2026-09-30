using System;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedType(Format.Native)]
public struct SeatCoordinate : INullable
{
    private bool isNull;
    private int row;
    private int col;
    private int seat;

    public bool IsNull => isNull;

    public static SeatCoordinate Null
    {
        get
        {
            SeatCoordinate item = new SeatCoordinate();
            item.isNull = true;
            return item;
        }
    }

    public int Row
    {
        get => row;
        set => row = value;
    }

    public int Col
    {
        get => col;
        set => col = value;
    }

    public int Seat
    {
        get => seat;
        set => seat = value;
    }

    public override string ToString()
    {
        if (isNull) 
            return "NULL";
            
        return $"{row}-{col}-{seat}";
    }

    public static SeatCoordinate Parse(SqlString s)
    {
        if (s.IsNull) return Null;

        string str = s.Value;
        string[] parts = str.Split('-');

        SeatCoordinate udt = new SeatCoordinate();
        udt.row = int.Parse(parts[0]);
        udt.col = int.Parse(parts[1]);
        udt.seat = int.Parse(parts[2]);
        udt.isNull = false;

        return udt;
    }
}