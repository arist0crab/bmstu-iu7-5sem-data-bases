-- 1. select с предикатом сравнения
-- Ученики класса с id = 3, у которых есть справки об отсутствии по причине "карантин"
SELECT S.first_name, S.last_name, A.reason, A.absent_date FROM students S JOIN absents AS A ON S.id = A.student_id WHERE S.class_group_id = 3 AND A.reason = 'Карантин'; 

-- 2. select c предикатом BETWEEN
-- Ученики, которые родились в 2016 году в период с 24 октября по 22 ноября
SELECT first_name, last_name, birth_date FROM students WHERE birth_date BETWEEN '2016-10-24' AND '2016-11-22';

-- 3. select с предикатом LIKE
-- Ученики, у которых есть диагноз "Миопия"
SELECT first_name, last_name FROM students JOIN medical_certificates ON students.id = medical_certificates.student_id WHERE diagnosis LIKE '%Миопия%';

-- 4. select с предикатом IN с вложенным подзапросом
-- Ученики-мальчики из 6-х классов
SELECT first_name, last_name FROM students WHERE class_group_id IN (SELECT id FROM class_groups WHERE grade = 6) AND sex = 'M';

-- 5. select с предикатом EXISTS с вложенным подзапросом
-- Ученики, у которых есть хотя бы одна неистекшая справка
SELECT first_name, last_name FROM students S WHERE EXISTS (SELECT * FROM medical_certificates M LEFT OUTER JOIN students ST ON ST.id = M.student_id WHERE ST.id = S.id AND M.expire_date IS NULL);

-- 6. select с предикатом сравнения с квантором
-- Номера кабинетов, в которых рядов парт больше, чем в любом кабинете с 6-ю колонками парт
SELECT number FROM cabinets WHERE rows_count > ALL (SELECT row_index FROM cabinets WHERE cols_count = 6);