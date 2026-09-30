using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server; 

public class QuarantineSimulation
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IsolateInfectedGroup(SqlInt32 studentId, SqlString diagnosis, SqlInt32 quarantineDurationDays, SqlDateTime startDate)
    {
        DateTime start = startDate.IsNull ? DateTime.Now : startDate.Value;

        List<DateTime> quarantineDates = new List<DateTime>();
        for (int i = 0; i < quarantineDurationDays.Value; i++)
        {
            DateTime currentDate = start.AddDays(i);
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                quarantineDates.Add(currentDate);
        }

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            SqlCommand updateStudentAssignments = new SqlCommand(
                @"
                UPDATE seatings_assignments
                SET end_date = @startDate
                WHERE student_id IN
                (
                    SELECT
                        id
                    FROM students 
                    WHERE class_group_id = (
                        SELECT class_group_id 
                        FROM students 
                        WHERE id = @studentId
                    )
                ) AND (end_date IS NULL OR end_date > @startDate)
                ", 
                conn
            );
            updateStudentAssignments.Parameters.AddWithValue("@studentId", studentId);
            updateStudentAssignments.Parameters.AddWithValue("@startDate", startDate);
            updateStudentAssignments.ExecuteNonQuery();

            SqlCommand getStudents = new SqlCommand(
                @"
                SELECT id 
                FROM students 
                WHERE class_group_id = (
                    SELECT class_group_id 
                    FROM students 
                    WHERE id = @studentId
                )
                ",
                conn
            );
            getStudents.Parameters.AddWithValue("@studentId", studentId);

            List<int> studentIds = new List<int>();
            using (SqlDataReader reader = getStudents.ExecuteReader())
            {
                while (reader.Read())
                    studentIds.Add(reader.GetInt32(0));
            }

            using (SqlTransaction transaction = conn.BeginTransaction())
            {
                SqlCommand insertAbsent = new SqlCommand(
                    @"
                    IF NOT EXISTS (
                        SELECT 1 FROM absents 
                        WHERE student_id = @studentId AND absent_date = @absentDate
                    )
                    BEGIN
                        INSERT INTO absents (student_id, absent_date, reason, status)
                        VALUES (@studentId, @absentDate, @reason, 'QUARANTINE')
                    END
                    ",
                    conn,
                    transaction
                );

                insertAbsent.Parameters.Add("@studentId", System.Data.SqlDbType.Int);
                insertAbsent.Parameters.Add("@absentDate", System.Data.SqlDbType.DateTime);
                insertAbsent.Parameters.Add("@reason", System.Data.SqlDbType.NVarChar);
                insertAbsent.Prepare();

                foreach (int sId in studentIds)
                {
                    foreach (DateTime date in quarantineDates)
                    {
                        insertAbsent.Parameters["@studentId"].Value = sId;
                        insertAbsent.Parameters["@absentDate"].Value = date;
                        insertAbsent.Parameters["@reason"].Value = diagnosis.IsNull ? "QUARANTINE" : diagnosis.Value;
                        insertAbsent.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }
    }
}