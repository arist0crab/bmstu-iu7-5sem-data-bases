SELECT
    extra -> 'contact_info' AS contact_info,
    extra -> 'academic_interests' AS academic_interests
FROM students LIMIT 5;

SELECT
    id,
    first_name || ' ' || last_name AS full_name,
    (extra -> 'contact_info') ->> 'telegram' AS telegram,
    (extra -> 'academic_interests') ->> 0 AS academic_interests
FROM students;

SELECT
    *
FROM students
WHERE extra ? 'skills';

UPDATE students
SET extra = jsonb_set(extra, '{preferred_language}', '"en"'::jsonb, false)
WHERE id = 2;

SELECT
    jsonb_array_elements(
        extra -> 'academic_interests'
    )
FROM students
WHERE extra ? 'academic_interests';