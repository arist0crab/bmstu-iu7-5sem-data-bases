ALTER TABLE students ADD COLUMN IF NOT EXISTS extra JSONB;

UPDATE students
SET extra = jsonb_build_object(
    'is_active', true,
    'preferred_language', 'ru',
    'contact_info', jsonb_build_object(
        'phone', '+7-999-' || lpad((id % 1000)::text, 3, '0') || '-1234',
        'telegram', '@student_' || id
    ),
    'academic_interests', jsonb_build_array('Databases', 'Software Engineering')
);

UPDATE students
SET extra = extra || jsonb_build_object(
    'has_scholarship', true,
    'skills', jsonb_build_array('C++', 'PostgreSQL', 'Linux'),
    'contacts', jsonb_build_object(
        'github', 'github.com/student_' || id
    )
)
WHERE id % 2 = 0;

UPDATE students
SET extra = extra || jsonb_build_object(
    'dormitory', jsonb_build_object(
        'building', (id % 5) + 1,
        'room', (id % 100) + 101
    )
)
WHERE id % 3 = 0;