-- 1. select с предикатом сравнения

SELECT S.first_name, S.last_name, A.absent_date FROM students S JOIN absents AS A ON S.id = A.student_id WHERE A.reason = 'Уважительная' AND S.class_group_id = 3; 