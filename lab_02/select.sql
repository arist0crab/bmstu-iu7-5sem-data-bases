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
SELECT number FROM cabinets WHERE rows_count > ALL (SELECT rows_count FROM cabinets WHERE cols_count = 6);

-- 7. select использующая агрегатные функции в выражениях столбцов
-- Среднее количество учеников в классе
SELECT AVG(TotalStudents) AS "Actual AVG students", SUM(TotalStudents) / COUNT(class_group_id) AS "Calculated AVG" FROM (SELECT class_group_id, COUNT(*) AS TotalStudents FROM students GROUP BY class_group_id);

-- 8. select использующая скалярные подзапросы в выражениях столбцов
-- ID, параллель класса, идентификатор класса, количество студентов в классе, количестве студентов с миопией в каждом классе
SELECT id, grade, internal_id, (SELECT COUNT(*) FROM students AS S WHERE S.class_group_id = CG.id) AS total_students, (SELECT COUNT(DISTINCT S.id) FROM students AS S JOIN medical_certificates AS M ON S.id = M.student_id WHERE S.class_group_id = CG.id AND M.diagnosis LIKE '%Миопия%') AS students_with_miopy FROM class_groups AS CG;

-- 9. select использующая простое выражение CASE
-- Ученики, которые не пропускали ни разу и те, кто пропустил хотя бы один раз
SELECT first_name AS "Имя", last_name AS "Фамилия", CASE COUNT(A.id) WHEN 0 THEN 'Автомат' ELSE 'Отчислить' END AS "Допуск" FROM students as S LEFT JOIN absents AS A ON S.id = A.student_id GROUP BY S.id;

-- 10. select использующая поисковое выражение CASE
-- Ученики, которые не пропускали ни разу, кто пропустил немного и кто пропустил больше 3х раз
SELECT first_name AS "Имя", last_name AS "Фамилия", CASE WHEN COUNT(A.id) = 0 THEN 'Автомат' WHEN COUNT(A.id) <= 3 THEN 'Не больше трех пропусков' ELSE 'Отчислить' END AS "Допуск" FROM students as S LEFT JOIN absents AS A ON S.id = A.student_id GROUP BY S.id;

-- 11. select с созданием ново временно локально таблицы из результирующего набора данных
-- Временная таблица, которая хранит id класса и кол-во учеников в нем 
DROP TABLE IF EXISTS class_students_quantity;
CREATE TEMP TABLE class_students_quantity SELECT CG.id, COUNT(S.id) FROM class_groups AS CG JOIN students AS S ON CG.id = S.class_group_id GROUP BY CG.id;

-- 12. select с вложенными коррелированными подзапросами в качестве производных таблиц в предложении from
-- Студент, который больше всех пропускал и который имеет больше всех справок
SELECT 'По пропускам' AS criteria, s.first_name, s.last_name AS top_student
FROM students s 
JOIN (
    SELECT student_id, COUNT(*) AS total_absents
    FROM absents
    GROUP BY student_id
    ORDER BY total_absents DESC
    LIMIT 1
) AS best_absent ON best_absent.student_id = s.id

UNION ALL

SELECT 'По справкам' AS criteria, s.first_name, s.last_name AS top_student
FROM students s 
JOIN (
    SELECT student_id, COUNT(*) AS total_certs
    FROM medical_certificates
    GROUP BY student_id
    ORDER BY total_certs DESC
    LIMIT 1
) AS best_cert ON best_cert.student_id = s.id;

-- 13. select с вложенными подзапросами с уровнем вложенности 3
-- Ученик с максимальным количеством пропусков

SELECT s.first_name, s.last_name AS worst_student
FROM students s
WHERE s.id = (
    SELECT a.student_id
    FROM absents a
    GROUP BY a.student_id
    HAVING COUNT(*) = (
        SELECT MAX(cnt)
        FROM (
            SELECT COUNT(*) AS cnt
            FROM absents
            GROUP BY student_id
        ) AS abs_counts
    )
);

-- 14. select консолидирующий данные с помощью предложения GROUP BY, но без предложения HAVING
-- Классы и их количество учеников
SELECT CG.grade, CG.letter_id, COUNT(S.id) FROM class_groups AS CG JOIN students AS S ON CG.id = S.class_group_id GROUP BY CG.id;

-- 15. select консолидирующий данные с помощью предложения GROUP BY и предложения HAVING
-- Классы, в которых количество учеников больше 4
SELECT CG.grade, CG.letter_id, COUNT(S.id) FROM class_groups AS CG JOIN students AS S ON CG.id = S.class_group_id GROUP BY CG.id HAVING COUNT(S.id) > 4;

-- 16. insert выполняющая вставку в таблицу одно строки значений
-- Вставляет новый кабинет в таблицу кабинетов
INSERT INTO cabinets (number, rows_count, cols_count) VALUES (676767, 5, 5);

-- 17. insert выполняющая вставку в таблицу результирующего набора данных вложенного подзапроса
-- Добавляет справку ученице Олимпиаде Романовой с минимальным id о том что у нее миопия последней стадии
INSERT INTO medical_certificates (student_id, diagnosis, issue_date, expire_date, recommended_zone) SELECT (SELECT MIN(id) FROM students WHERE first_name = 'Олимпиада' AND last_name = 'Романова'), 'Миопия высокой степени', '2026-08-28', NULL, NULL;

-- 18. update
-- Расширяет одинарные парты до двухместных 
UPDATE desks SET seats_count = 2 WHERE seats_count = 1;

-- 19. update со скалярным подзапросом в предложении SET
-- Усредняет количество мест за всеми партами, где было больше 2х мест
UPDATE desks SET seats_count = (SELECT AVG(seats_count) FROM desks WHERE seats_count > 2) WHERE seats_count > 2;

-- 20. delete
-- Удаление всех здоровых студентов
DELETE FROM medical_certificates WHERE diagnosis LIKE '%Здоров%';

-- 21. delete с вложенным коррелированным подзапросом в предложении WHERE
-- Удаляет медицинские справки всех мальчиков из 5х классов
DELETE FROM medical_certificates WHERE student_id IN (SELECT S.id FROM students as S JOIN class_groups AS CG ON S.class_group_id = CG.id WHERE S.sex = 'M' AND CG.grade = 5);

-- 22. select с простым обобщенным табличным выражением
-- Создает таблицу, храняющую количество парт в каждом кабинете, считает среднее количество парт на кабинет
WITH CPC (cabinet_id, desks_quantity) AS (SELECT cabinet_id, COUNT(*) FROM desks GROUP BY cabinet_id) SELECT AVG (desks_quantity) AS "Срднее количество парт в кабинете" FROM CPC;

-- 23. select с рекурсивным обобщенным табличным выражением
-- Создает плоскую таблицу менторов учителей
WITH RECURSIVE TeacherMentors (mentor_teacher_id, id, first_name, last_name, level) AS (
    SELECT mentor_teacher_id, id, first_name, last_name, 0 AS level
    FROM teachers
    WHERE mentor_teacher_id IS NULL

    UNION ALL

    SELECT T.mentor_teacher_id, T.id, T.first_name, T.last_name, TM.level + 1
    FROM teachers AS T INNER JOIN TeacherMentors AS TM ON T.mentor_teacher_id = TM.id
    )
SELECT * FROM TeacherMentors ORDER BY level;

-- 24. Оконные функции. Использование конструкци MIN/MAX/AVG OVER()
-- Выводит ФИО студента, число его справок и среднее число справок по его классу
SELECT
    first_name,
    last_name,
    grade,
    letter_id,
    cert_count,
    AVG(cert_count) OVER (PARTITION BY class_group_id) AS avg_in_class
FROM (
    SELECT
        S.id,
        S.first_name,
        S.last_name,
        S.class_group_id,
        CG.grade,
        CG.letter_id,
        COUNT(MC.id) AS cert_count
    FROM students AS S
    LEFT JOIN medical_certificates AS MC ON MC.student_id = S.id
    LEFT JOIN class_groups AS CG ON CG.id = S.class_group_id
    GROUP BY S.id, S.first_name, S.last_name, S.class_group_id, CG.grade, CG.letter_id
) AS Sub
ORDER BY grade, letter_id, last_name;

-- 25. Запрос, в результате которого в данных появляются полные дубли. Устраняет дублирующиеся строки с использованием функции ROW_NUMBER()

SELECT setval(
    pg_get_serial_sequence('seats', 'id'),
    COALESCE((SELECT MAX(id) FROM seats), 1)
);

-- Создание дубликатор первых 100 записей
INSERT INTO seats (desk_id, seat_index)
SELECT desk_id, seat_index
FROM seats
CROSS JOIN generate_series(1, 3)
WHERE id <= 100;

-- Удаление дублей
WITH NewDuplicates AS (
    SELECT 
        s.id,
        ROW_NUMBER() OVER (PARTITION BY s.desk_id, s.seat_index ORDER BY s.id) AS rn
    FROM seats s
    WHERE s.id > 1000
)
DELETE FROM seats
WHERE id IN (
    SELECT id 
    FROM NewDuplicates
);

-- защита
-- Найти учителей, у которых в классе нет детей с диагнозом 'Здоров'

SELECT T.id, T.first_name, T.last_name 
FROM teachers AS T JOIN class_groups AS CG ON CG.teacher_id = T.id
WHERE NOT EXISTS (
    SELECT *
    FROM students AS S
    JOIN medical_certificates AS MC ON MC.student_id = S.id
    WHERE S.class_group_id = CG.id AND MC.diagnosis = 'Здоров'
);