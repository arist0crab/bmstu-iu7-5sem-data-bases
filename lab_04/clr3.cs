using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class SeatingAssignmentGenerator
{
    private class SeatingAssignment(
        SqlInt32 studentId, 
        SqlString firstName, 
        SqlString lastName, 
        SqlInt32 seatId
    )
    {
        public SqlInt32 StudentId { get; set; } = studentId;
        public SqlString FirstName { get; set; } = firstName;
        public SqlString LastName { get; set; } = lastName;
        public SqlInt32 SeatId { get; set; } = seatId;
    }

    [SqlFunction(FillRowMethodName = "FillRow", DataAccess = DataAccessKind.Read)]
    public static IEnumerable InitMethod(SqlInt32 classGroupId)
    {
        List<SeatingAssignment> resultCollection = [];

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT S.id as student_id, S.first_name, S.last_name, SA.seat_id FROM students as S JOIN seatings_assignments as SA ON S.id = SA.student_id WHERE S.class_group_id = @classGroupId", conn);
            cmd.Parameters.AddWithValue("@classGroupId", classGroupId);

            using SqlDataReader seatingAssignmentReader = cmd.ExecuteReader();
            ReadSeatingAssignments(resultCollection, seatingAssignmentReader);
            ShuffleSeatingAssignments(resultCollection);
        }

        return resultCollection;
    }   

    public static void FillRow(
        object obj,
        out SqlInt32 studentId, 
        out SqlString studentFirstName, 
        out SqlString studentLastName,
        out SqlInt32 seatId
    )
    {
        SeatingAssignment sa = (SeatingAssignment)obj;
        studentId = sa.StudentId;
        studentFirstName = sa.FirstName;
        studentLastName = sa.LastName;
        seatId = sa.SeatId;
    }

    private static void ReadSeatingAssignments(List<SeatingAssignment> resultCollection, SqlDataReader seatingAssignmentReader)
    {
        while (seatingAssignmentReader.Read())
        {
            resultCollection.Add(
                new SeatingAssignment(
                    seatingAssignmentReader.GetOrdinal("student_id"), 
                    seatingAssignmentReader.GetOrdinal("first_name"), 
                    seatingAssignmentReader.GetOrdinal("last_name"), 
                    seatingAssignmentReader.GetOrdinal("seat_id")
                )
            );
        }
    }

    private static void ShuffleSeatingAssignments(List<SeatingAssignment> resultCollection)
    {
        List<SqlInt32> seats = new(resultCollection.Count);
        
        foreach(var sa in resultCollection)
            seats.Add(sa.SeatId);

        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(seats));

        for (nint i = 0; i < resultCollection.Count; ++i)
            resultCollection[i].SeatId = seats[i];
    }
}