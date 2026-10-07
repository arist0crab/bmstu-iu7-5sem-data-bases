using System;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public class UninfectedStudentsDetector
{
    private class StudentSeat
    {
        public SqlInt32 StudentId { get; set; }
        public SqlString FirstName { get; set; }
        public SqlString LastName { get; set; }
        public SqlInt32 RowIndex { get; set; }
        public SqlInt32 ColIndex { get; set; }
        public SqlInt32 SeatId { get; set; }

        public StudentSeat(SqlInt32 studentId, SqlString firstName, SqlString lastName, SqlInt32 rowIndex, SqlInt32 colIndex, SqlInt32 seatId)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            RowIndex = rowIndex;
            ColIndex = colIndex;
            SeatId = seatId;
        }
    }


    [SqlFunction(FillRowMethodName = "FillRow", DataAccess = DataAccessKind.Read)]
    public static IEnumerable InitMethod(SqlInt32 studentId)
    {
        if (studentId.IsNull)
            return null;

        StudentSeat infectedStudent = null;
        List<StudentSeat> classSeats = new List<StudentSeat>();
        List<StudentSeat> safeStudents = new List<StudentSeat>();

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            string query = @"
                SELECT 
                    ST.id AS student_id
                    , ST.first_name 
                    , ST.last_name
                    , D.row_index
                    , D.col_index
                    , SE.id AS seat_id
                FROM students AS ST
                JOIN seatings_assignments AS SA ON ST.id = SA.student_id
                JOIN seats AS SE ON SA.seat_id = SE.id
                JOIN desks AS D ON SE.desk_id = D.id
                WHERE ST.class_group_id = (
                    SELECT class_group_id FROM students WHERE id = @studentId
                );
            ";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@studentId", studentId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudentSeat ss = new StudentSeat(
                            reader.GetSqlInt32(0),
                            reader.GetSqlString(1),
                            reader.GetSqlString(2),
                            reader.GetSqlInt32(3),
                            reader.GetSqlInt32(4),
                            reader.GetSqlInt32(5)
                        );

                        if (ss.StudentId.Value == studentId.Value)
                            infectedStudent = ss;

                        classSeats.Add(ss);
                    }
                }
            }

            if (infectedStudent == null)
                return null;

            foreach (StudentSeat student in classSeats)
            {
                if (student.StudentId.Value == infectedStudent.StudentId.Value)
                    continue;

                bool isAtRisk = Math.Abs(student.RowIndex.Value - infectedStudent.RowIndex.Value) <= 1 && Math.Abs(student.ColIndex.Value - infectedStudent.ColIndex.Value) <= 1;

                if (!isAtRisk)
                    safeStudents.Add(student);
            }
        }

        return safeStudents;
    } 

    public static void FillRow(object obj, out SqlInt32 studentId, out SqlString firstName, out SqlString lastName, out SqlInt32 rowIndex, out SqlInt32 colIndex, out SqlInt32 seatId)
    {
        StudentSeat student = (StudentSeat)obj;
        studentId = student.StudentId;
        firstName = student.FirstName;
        lastName = student.LastName;
        rowIndex = student.RowIndex;
        colIndex = student.ColIndex;
        seatId = student.SeatId;
    }
} 
