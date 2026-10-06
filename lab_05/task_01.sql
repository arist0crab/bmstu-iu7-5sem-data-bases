SELECT
    jsonb_pretty(jsonb_agg(to_jsonb(t)))
    AS teachers_json
FROM teachers t;

SELECT jsonb_pretty(
    jsonb_agg(
        jsonb_build_object(
            'student_id', S.id,
            'full_name', S.first_name || ' ' || S.last_name,
            'absences', (
                SELECT coalesce(
                    jsonb_agg(
                        jsonb_build_object(
                            'absent_date', A.absent_date,
                            'reason', A.reason
                        )
                    ),
                    '[]'::jsonb
                )
                FROM absents A
                WHERE S.id = A.student_id
            )
        )
    )
) AS students_with_absences_json
FROM students AS S
LIMIT 10;