using System;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public class MedicalCertificateTriggers
{
    [SqlTrigger(Name = "TR_MedicalCertificates_OnInsert", Target = "medical_certificates", Event = "FOR INSERT")]
    public static void OnMedicalCertificateAdded()
    {
        SqlCommand command;
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;

        if (triggerContext.TriggerAction == TriggerAction.Insert)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                command = new SqlCommand(
                    @"
                    UPDATE seatings_assignments
                    SET end_date = CAST(GETDATE() AS DATE)
                    WHERE student_id IN (SELECT student_id FROM inserted)
                      AND (end_date IS NULL OR end_date > CAST(GETDATE() AS DATE));
                    ",
                    connection
                );

                command.ExecuteNonQuery();
            }
        }
    }
}