-- 1. select с предикатом сравнения
-- Ученики класса с id = 3, у которых есть справки об отсутствии по причине "карантин"
SELECT S.first_name, S.last_name, A.reason, A.absent_date FROM students S JOIN absents AS A ON S.id = A.student_id WHERE S.class_group_id = 3 AND A.reason = 'Карантин'; 

-- 2. select c предикатом BETWEEN
