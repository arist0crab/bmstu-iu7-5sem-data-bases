using System;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;


public class AgeCalculation
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlInt32 CalculateAge(SqlDateTime birthDate, SqlDateTime targetDate)
    {
        if (birthDate.IsNull || targetDate.IsNull)
            return SqlInt32.Null;

        DateTime bd = birthDate.Value;
        DateTime td = targetDate.Value;

        if (bd > td)
            return SqlInt32.Null;

        int years = td.Year - bd.Year;
        years -= (td < bd.AddYears(years)) ? 1 : 0;
        
        return (SqlInt32)years;
    }
}