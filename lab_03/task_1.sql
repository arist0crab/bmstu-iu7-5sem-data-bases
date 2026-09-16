-- Скалярная фнукция 
-- Возвращает количество учеников в одном конкретном классе

CREATE OR REPLACE FUNCTION calc_students_in_class(target_id INT)
RETURNS INT AS $main$
BEGIN
    RETURN (SELECT COUNT(*) FROM students WHERE class_group_id = target_id);
END;
$main$
LANGUAGE plpgsql;

SELECT calc_students_in_class(67);