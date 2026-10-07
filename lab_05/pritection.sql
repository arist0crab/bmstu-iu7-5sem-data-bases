SELECT jsonb_pretty(
    jsonb_agg(
        jsonb_build_object(
            'student_id', S.id,
            'full_name', S.first_name || ' ' || S.last_name,
            'absences', (
                SELECT
                    jsonb_agg(reason)
                FROM absents A
                WHERE S.id = A.student_id
            )
        )
    )
) AS students_with_absences_json
FROM students AS S
WHERE EXISTS (SELECT * FROM absents AS A WHERE S.id = A.student_id);