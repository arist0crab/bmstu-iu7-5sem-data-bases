import os
import csv
import random
from datetime import timedelta
from faker import Faker

fake = Faker('ru_RU')

NUM_RECORDS = 1000

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
OUTPUT_DIR = os.path.join(SCRIPT_DIR, "output_csv")
os.makedirs(OUTPUT_DIR, exist_ok=True)


def get_random_id(id_list, none_chance=0.1):
    if not id_list or random.random() < none_chance:
        return None
    return random.choice(id_list)


def generate_teachers():
    print("Генерация teachers...")
    data = []
    teacher_ids = []

    for i in range(1, NUM_RECORDS + 1):
        teacher_ids.append(i)
        birth_date = fake.date_of_birth(minimum_age=25, maximum_age=65)
        
        mentor_id = None
        while True:
            mentor_id = get_random_id(teacher_ids, none_chance=0.3)
            if mentor_id is None or mentor_id != i:
                break
        
        data.append({
            "id": i,
            "first_name": fake.first_name(),
            "last_name": fake.last_name(),
            "birth_date": birth_date.strftime("%Y-%m-%d"),
            "mentor_teacher_id": mentor_id
        })

    return data, teacher_ids


def generate_cabinets():
    print("Генерация cabinets...")
    data = []
    cabinet_ids = []
    used_numbers = set()

    for i in range(1, NUM_RECORDS + 1):
        cabinet_ids.append(i)
        
        while True:
            number = random.randint(1, NUM_RECORDS * 2)
            if number not in used_numbers:
                used_numbers.add(number)
                break
        
        data.append({
            "id": i,
            "number": number,
            "rows_count": random.randint(3, 8),
            "cols_count": random.randint(4, 10)
        })
    return data, cabinet_ids


def generate_class_groups(teacher_ids, cabinet_ids):
    print("Генерация class_groups...")
    data = []
    class_group_ids = []
    letters = ['А', 'Б', 'В', 'Г', 'Д', 'Е']
    
    used_pairs = set()

    for i in range(1, NUM_RECORDS + 1):
        class_group_ids.append(i)
        
        while True:
            teacher_id = get_random_id(teacher_ids, none_chance=0.05)
            cabinet_id = get_random_id(cabinet_ids, none_chance=0.1)
            pair = (teacher_id, cabinet_id)
            if pair not in used_pairs:
                used_pairs.add(pair)
                break
        
        data.append({
            "id": i,
            "grade": random.randint(1, 11),
            "internal_id": random.randint(1, 11),
            "letter_id": random.choice(letters),
            "teacher_id": teacher_id,
            "cabinet_id": cabinet_id
        })
    return data, class_group_ids


def generate_students(class_group_ids):
    print("Генерация students...")
    data = []
    student_ids = []

    for i in range(1, NUM_RECORDS + 1):
        student_ids.append(i)
        birth_date = fake.date_of_birth(minimum_age=6, maximum_age=18)
        sex = random.choice(['M', 'F'])

        data.append({
            "id": i,
            "first_name": fake.first_name_male() if sex == 'M' else fake.first_name_female(),
            "last_name": fake.last_name_male() if sex == 'M' else fake.last_name_female(),
            "sex": sex,
            "birth_date": birth_date.strftime("%Y-%m-%d"),
            "class_group_id": random.choice(class_group_ids),
            "is_active": random.choices([True, False], weights=[0.9, 0.1])[0]
        })
    return data, student_ids


def generate_medical_certificates(student_ids):
    print("Генерация medical_certificates...")
    data = []

    diagnoses = [
        'Здоров',
        'Миопия слабой степени',
        'Миопия средней степени',
        'Миопия высокой степени',
        'Астигматизм',
        'Дальнозоркость',
        'Спазм аккомодации',
        'Косоглазие',
        'Амблиопия'
    ]

    MAX_RECOMMENDED_ROWS = 5

    for i in range(1, NUM_RECORDS + 1):
        issue_date = fake.date_between(start_date='-2y', end_date='today')
        expire_date = issue_date + timedelta(days=random.randint(180, 365))

        diag = random.choice(diagnoses)

        if diag == 'Здоров':
            zone = None
        else:
            allowed_rows_count = random.randint(1, MAX_RECOMMENDED_ROWS)
            zone_list = list(range(1, allowed_rows_count + 1))
            zone = "{" + ",".join(map(str, zone_list)) + "}"

        data.append({
            "id": i,
            "student_id": random.choice(student_ids),
            "diagnosis": diag,
            "issue_date": issue_date.strftime("%Y-%m-%d"),
            "expire_date": expire_date.strftime("%Y-%m-%d"),
            "recommended_zone": zone
        })
    return data


def generate_absents(student_ids):
    print("Генерация absents...")
    data = []
    reasons = ['Болезнь (ОРВИ)', 'Семейные обстоятельства', 'Поездка', 'Неизвестно', 'Карантин']
    statuses = ['Уважительная', 'Неуважительная', 'В процессе уточнения']

    for i in range(1, NUM_RECORDS + 1):
        absent_date = fake.date_between(start_date='-8m', end_date='today')

        data.append({
            "id": i,
            "student_id": random.choice(student_ids),
            "absent_date": absent_date.strftime("%Y-%m-%d"),
            "reason": random.choice(reasons),
            "status": random.choice(statuses)
        })
    return data


def generate_desks(cabinet_ids):
    print("Генерация desks...")
    data = []
    desk_ids = []

    for i in range(1, NUM_RECORDS + 1):
        desk_ids.append(i)
        data.append({
            "id": i,
            "cabinet_id": random.choice(cabinet_ids),
            "row_index": random.randint(1, 8),
            "col_index": random.randint(1, 10),
            "seats_count": random.choice([1, 2])
        })
    return data, desk_ids


def generate_seats(desk_ids):
    print("Генерация seats...")
    data = []
    seat_ids = []

    for i in range(1, NUM_RECORDS + 1):
        seat_ids.append(i)
        data.append({
            "id": i,
            "desk_id": random.choice(desk_ids),
            "seat_index": random.choice([1, 2])
        })
    return data, seat_ids


def generate_seatings_assignments(student_ids, seat_ids):
    print("Генерация seatings_assignments...")
    data = []

    for i in range(1, NUM_RECORDS + 1):
        start_date = fake.date_between(start_date='-1y', end_date='today')

        if random.random() < 0.7:
            end_date = None
        else:
            end_date = start_date + timedelta(days=random.randint(30, 180))

        data.append({
            "id": i,
            "student_id": random.choice(student_ids),
            "seat_id": random.choice(seat_ids),
            "start_date": start_date.strftime("%Y-%m-%d"),
            "end_date": end_date.strftime("%Y-%m-%d") if end_date else None
        })
    return data


def save_to_csv(filename, data, fieldnames):
    filepath = os.path.join(OUTPUT_DIR, filename)
    with open(filepath, mode='w', encoding='utf-8-sig', newline='') as f:
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        for row in data:
            clean_row = {}
            for k, v in row.items():
                if v is None:
                    clean_row[k] = "NULL"
                elif isinstance(v, bool):
                    clean_row[k] = "true" if v else "false"
                else:
                    clean_row[k] = v
            writer.writerow(clean_row)
    print(f"  Сохранено: {filepath} ({len(data)} записей)")


if __name__ == "__main__":
    print("Начало генерации данных...\n")

    teachers_data, teacher_ids = generate_teachers()
    cabinets_data, cabinet_ids = generate_cabinets()
    class_groups_data, class_group_ids = generate_class_groups(teacher_ids, cabinet_ids)
    students_data, student_ids = generate_students(class_group_ids)
    med_certs_data = generate_medical_certificates(student_ids)
    absents_data = generate_absents(student_ids)
    desks_data, desk_ids = generate_desks(cabinet_ids)
    seats_data, seat_ids = generate_seats(desk_ids)
    seatings_data = generate_seatings_assignments(student_ids, seat_ids)

    print("\nСохранение в CSV файлы...")

    save_to_csv("teachers.csv", teachers_data,
                ["id", "first_name", "last_name", "birth_date", "mentor_teacher_id"])
    save_to_csv("cabinets.csv", cabinets_data,
                ["id", "number", "rows_count", "cols_count"])
    save_to_csv("class_groups.csv", class_groups_data,
                ["id", "grade", "internal_id", "letter_id", "teacher_id", "cabinet_id"])
    save_to_csv("students.csv", students_data,
                ["id", "first_name", "last_name", "sex", "birth_date", "class_group_id", "is_active"])
    save_to_csv("medical_certificates.csv", med_certs_data,
                ["id", "student_id", "diagnosis", "issue_date", "expire_date", "recommended_zone"])
    save_to_csv("absents.csv", absents_data,
                ["id", "student_id", "absent_date", "reason", "status"])
    save_to_csv("desks.csv", desks_data,
                ["id", "cabinet_id", "row_index", "col_index", "seats_count"])
    save_to_csv("seats.csv", seats_data,
                ["id", "desk_id", "seat_index"])
    save_to_csv("seatings_assignments.csv", seatings_data,
                ["id", "student_id", "seat_id", "start_date", "end_date"])

    print(f"\nГенерация завершена! Файлы в: {OUTPUT_DIR}")