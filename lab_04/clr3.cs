using System;
using System.Collections; 
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

public class SeatingAssignmentGenerator
{
    private class SeatingAssignment
    {
        public SqlInt32 StudentId { get; set; }
        public SqlString FirstName { get; set; }
        public SqlString LastName { get; set; }
        public SqlInt32 SeatId { get; set; }

        public SeatingAssignment(
            SqlInt32 studentId,
            SqlString firstName,
            SqlString lastName,
            SqlInt32 seatId)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            SeatId = seatId;
        }
    }

    [SqlFunction(FillRowMethodName = "FillRow", DataAccess = DataAccessKind.Read)]
    public static IEnumerable InitMethod(SqlInt32 classGroupId)
    {
        List<SeatingAssignment> resultCollection = new List<SeatingAssignment>();

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(
                @"SELECT S.id as student_id, S.first_name, S.last_name, SA.seat_id 
                  FROM students as S 
                  JOIN seatings_assignments as SA ON S.id = SA.student_id 
                  WHERE S.class_group_id = @classGroupId", conn))
            {
                cmd.Parameters.AddWithValue("@classGroupId", classGroupId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ReadSeatingAssignments(resultCollection, reader);
                }
            }
        }

        ShuffleSeatingAssignments(resultCollection);
        return resultCollection;
    }

    public static void FillRow(
        object obj,
        out SqlInt32 studentId,
        out SqlString studentFirstName,
        out SqlString studentLastName,
        out SqlInt32 seatId)
    {
        SeatingAssignment sa = (SeatingAssignment)obj;
        studentId = sa.StudentId;
        studentFirstName = sa.FirstName;
        studentLastName = sa.LastName;
        seatId = sa.SeatId;
    }

    private static void ReadSeatingAssignments(
        List<SeatingAssignment> resultCollection,
        SqlDataReader reader)
    {
        int studentIdIdx = reader.GetOrdinal("student_id");
        int firstNameIdx = reader.GetOrdinal("first_name");
        int lastNameIdx = reader.GetOrdinal("last_name");
        int seatIdIdx = reader.GetOrdinal("seat_id");

        while (reader.Read())
        {
            resultCollection.Add(
                new SeatingAssignment(
                    reader.GetSqlInt32(studentIdIdx),
                    reader.GetSqlString(firstNameIdx),
                    reader.GetSqlString(lastNameIdx),
                    reader.GetSqlInt32(seatIdIdx)
                )
            );
        }
    }

    private static void ShuffleSeatingAssignments(List<SeatingAssignment> resultCollection)
    {
        List<SqlInt32> seats = new List<SqlInt32>(resultCollection.Count);

        foreach (var sa in resultCollection)
            seats.Add(sa.SeatId);

        Random rnd = new Random();
        for (int i = seats.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            SqlInt32 tmp = seats[i];
            seats[i] = seats[j];
            seats[j] = tmp;
        }

        for (int i = 0; i < resultCollection.Count; ++i)
            resultCollection[i].SeatId = seats[i];
    }
}