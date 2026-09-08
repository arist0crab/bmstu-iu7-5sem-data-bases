import os
import csv
import random
import json
from datetime import datetime, timedelta
from faker import Faker

fake = Faker('ru_RU')

NUM_RECORDS = 1000

OUTPUT_DIR = "output_csv"
os.makedirs(OUTPUT_DIR, exist_ok=True)

def get_random_id(id_list, none_chance=0.1):
    if not id_list or random.random() < none_chance:
        return 0
    return random.choice(id_list)

def generate_teachers():
    print("Генерация Teacher...")
    data = []
    teacher_ids = []
    
    for i in range(1, NUM_RECORDS + 1):
        teacher_ids.append(i)
        birth_date = fake.date_of_birth(minimum_age=25, maximum_age=65)
        data.append({
            "ID": i,
            "first_name": fake.first_name(),
            "last_name": fake.last_name(),
            "birth_date": birth_date.strftime("%Y-%m-%d"),
            "class_group": 0,
            "mentor": 0
        })
    
    for row in data:
        row["mentor"] = get_random_id(teacher_ids, none_chance=0.2)
        
    return data, teacher_ids

def generate_cabinets():
    print("Генерация Cabinet...")
    data = []
    cabinet_ids = []
    
    for i in range(1, NUM_RECORDS + 1):
        cabinet_ids.append(i)
        data.append({
            "ID": i,
            "class_group": 0,
            "teacher": 0,
            "rows_count": random.randint(3, 8),
            "cols_count": random.randint(4, 10)
        })
    return data, cabinet_ids

def generate_class_groups(teacher_ids, cabinet_ids):
    print("Генерация ClassGroup...")
    data = []
    class_group_ids = []
    letters = ['А', 'Б', 'В', 'Г', 'Д', 'Е']
    
    for i in range(1, NUM_RECORDS + 1):
        class_group_ids.append(i)
        data.append({
            "ID": i,
            "number": random.randint(1, 11),
            "number_id": random.randint(1, 11),
            "letter_id": random.choice(letters),
            "teacher": get_random_id(teacher_ids, none_chance=0.05),
            "cabinet": get_random_id(cabinet_ids, none_chance=0.1)
        })
    return data, class_group_ids

def generate_students(class_group_ids):
    print("Генерация Student...")
    data = []
    student_ids = []
    
    for i in range(1, NUM_RECORDS + 1):
        student_ids.append(i)
        birth_date = fake.date_of_birth(minimum_age=6, maximum_age=18)
        sex = random.choice(['М', 'Ж'])
        
        data.append({
            "ID": i,
            "first_name": fake.first_name_male() if sex == 'М' else fake.first_name_female(),
            "last_name": fake.last_name_male() if sex == 'М' else fake.last_name_female(),
            "birth_date": birth_date.strftime("%Y-%m-%d"),
            "sex": sex,
            "class_group": get_random_id(class_group_ids, none_chance=0.01),
            "certificate": random.choice(['Нет', 'Обычный', 'С отличием', 'Похвальный лист']),
            "is_active": random.choices([1, 0], weights=[0.9, 0.1])[0]
        })
    return data, student_ids

def generate_medical_certificates(student_ids):
    print("Генерация MedicalCertificate...")
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
        start_date = fake.date_between(start_date='-2y', end_date='today')
        expire_date = start_date + timedelta(days=random.randint(180, 365))
        
        diag = random.choice(diagnoses)
        
        if diag == 'Здоров':
            zone = None
        else:
            allowed_rows_count = random.randint(1, MAX_RECOMMENDED_ROWS)
            zone_list = list(range(1, allowed_rows_count + 1))
            zone = "{" + ",".join(map(str, zone_list)) + "}"
        
        data.append({
            "ID": i,
            "student": get_random_id(student_ids, none_chance=0.05),
            "diagnosis": diag,
            "date": start_date.strftime("%Y-%m-%d"),
            "expire_date": expire_date.strftime("%Y-%m-%d"),
            "recommended_zone": zone
        })
    return data

def generate_absents(student_ids):
    print("Генерация Absent...")
    data = []
    reasons = ['Болезнь (ОРВИ)', 'Семейные обстоятельства', 'Поездка', 'Неизвестно', 'Карантин']
    statuses = ['Уважительная', 'Неуважительная', 'В процессе уточнения']
    
    for i in range(1, NUM_RECORDS + 1):
        absent_date = fake.date_between(start_date='-8m', end_date='today')
        
        data.append({
            "ID": i,
            "student": get_random_id(student_ids, none_chance=0.01),
            "date": absent_date.strftime("%Y-%m-%d"),
            "reason": random.choice(reasons),
            "status": random.choice(statuses)
        })
    return data

def generate_desks(cabinet_ids):
    print("Генерация Desk...")
    data = []
    desk_ids = []
    
    for i in range(1, NUM_RECORDS + 1):
        desk_ids.append(i)
        data.append({
            "ID": i,
            "cabinet": get_random_id(cabinet_ids, none_chance=0.05),
            "row_ind": random.randint(1, 8),
            "col_ind": random.randint(1, 10),
            "seats_count": random.choice([1, 2])
        })
    return data, desk_ids

def generate_seats(desk_ids):
    print("Генерация Seat...")
    data = []
    
    for i in range(1, NUM_RECORDS + 1):
        data.append({
            "ID": i,
            "desk": get_random_id(desk_ids, none_chance=0.01),
            "seat_index": random.choice([0, 1])
        })
    return data

def save_to_csv(filename, data, fieldnames):
    filepath = os.path.join(OUTPUT_DIR, filename)
    with open(filepath, mode='w', encoding='utf-8-sig', newline='') as f:
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        for row in data:
            clean_row = {k: ("NULL" if v is None else v) for k, v in row.items()}
            writer.writerow(clean_row)
    print(f"Сохранено: {filepath} ({len(data)} записей)")

if __name__ == "__main__":
    print("Начало генерации данных...")
    
    teachers_data, teacher_ids = generate_teachers()
    cabinets_data, cabinet_ids = generate_cabinets()
    
    class_groups_data, class_group_ids = generate_class_groups(teacher_ids, cabinet_ids)
    students_data, student_ids = generate_students(class_group_ids)
    
    med_certs_data = generate_medical_certificates(student_ids)
    absents_data = generate_absents(student_ids)
    desks_data, desk_ids = generate_desks(cabinet_ids)
    seats_data = generate_seats(desk_ids)
    
    print("\nСохранение в CSV файлы...")
    
    save_to_csv("Teacher.csv", teachers_data, 
                ["ID", "first_name", "last_name", "birth_date", "class_group", "mentor"])
    save_to_csv("Cabinet.csv", cabinets_data, 
                ["ID", "class_group", "teacher", "rows_count", "cols_count"])
    save_to_csv("ClassGroup.csv", class_groups_data, 
                ["ID", "number", "number_id", "letter_id", "teacher", "cabinet"])
    save_to_csv("Student.csv", students_data, 
                ["ID", "first_name", "last_name", "birth_date", "sex", "class_group", "certificate", "is_active"])
    save_to_csv("MedicalCertificate.csv", med_certs_data, 
                ["ID", "student", "diagnosis", "date", "expire_date", "recommended_zone"])
    save_to_csv("Absent.csv", absents_data, 
                ["ID", "student", "date", "reason", "status"])
    save_to_csv("Desk.csv", desks_data, 
                ["ID", "cabinet", "row_ind", "col_ind", "seats_count"])
    save_to_csv("Seat.csv", seats_data, 
                ["ID", "desk", "seat_index"])
                
    print("\nГенерация успешно завершена! Файлы находятся в папке 'output_csv'.")