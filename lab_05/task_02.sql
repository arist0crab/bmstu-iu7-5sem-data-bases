DROP TABLE IF EXISTS teachers_restored;

CREATE TABLE "public".teachers_restored(
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    birth_date DATE,
    mentor_teacher_id INT
);

INSERT INTO teachers_restored (id, first_name, last_name, birth_date, mentor_teacher_id)
SELECT
    (item ->> 'id')::INT,
    item ->> 'first_name',
    item ->> 'last_name',
    (item ->> 'birth_date')::DATE,
    (item ->> 'mentor_teacher_id')::INT
FROM jsonb_array_elements(
    (pg_read_file('/tmp/teachers_json_text.json')::jsonb -> 0 ->> 'teachers_json')::jsonb)
AS item;

SELECT setval('teachers_restored_id_seq', (SELECT MAX(id) FROM teachers_restored));