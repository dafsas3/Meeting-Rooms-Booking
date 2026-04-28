## Архітектура

Я використовую **Vertical Slice Architecture** з розділенням на слої всередині кожної фічі.

Проєкт не є строгою Clean Architecture з окремими проєктами, але основні принципи дотримані:

- Domain не залежить від інших слоїв
- Application залежить від Domain та абстракцій
- Infrastructure реалізує ці абстракції

У проєкті використано CQRS та DDD patterns.

### CQRS

CQRS використовується для розділення читання та запису:

- Commands створюють або змінюють ресурс
- Queries тільки повертають існуючі дані

Також це дозволяє писати ефективні SQL-запити, наприклад через проєкції `Select`, і контролювати, які саме дані потрібно повернути.

### DDD

Бізнес-логіка знаходиться в домені, а не в обробниках.

Наприклад:

- `BookingRequest` сам контролює можливі переходи статусів через `BookingWorkflow`
- перевірка ролі та власника броні знаходиться в домені
- історія змін створюється автоматично при зміні статусу.

Таким чином:

- неможливо змінити стан об'єкта напряму
- всі інваріанти контролюються доменом
- handler виступає лише як оркестратор

---

## База даних та конкурентність

Для захисту від одночасної зміни одного й того ж рядка використовується `xmin` — optimistic concurrency PostgreSQL.

Перевірка перетинів тайм-слотів виконується SQL-запитом на стороні БД, без завантаження всіх бронювань у памʼять.

`xmin` захищає від одночасної зміни одного BookingRequest, але не гарантує повний захист від race condition між двома різними BookingRequest, якщо обидва запити одночасно не бачать конфлікту перетину.

Для створення BookingRequest використовується `Idempotency-Key` + унікальний індекс + retry. Це захищає від повторного створення одного й того ж ресурсу при повторних запитах клієнта.

---

## Запуск, міграції, конфігурація

### Вимоги

- .NET 8 SDK
- PostgreSQL
- dotnet-ef

### Конфігурація

Створіть файл `appsettings.Development.json` у проєкті `MeetingRoomsBooking`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=meeting_rooms_booking;Username=postgres;Password=your_password"
  }
}
```
Замініть your_password на реальний пароль користувача PostgreSQL.

### Міграції
- Перейдіть у папку проєкту: cd MeetingRoomsBooking
- Застосуйте міграції: dotnet ef database update

### Запуск API
У папці проєкту: dotnet run

Swagger буде доступний за адресою:
```
https://localhost:{port}/swagger
```
Актуальний порт буде виведений у консолі після запуску.

---

## Приклади API-запитів
Усі права перевіряються через headers:
- X-EmployeeID: {guid}
- X-UserRole: Employee | Admin

BookingStatus
- 0 = Draft
- 1 = Submitted
- 2 = Confirmed
- 3 = Declined
- 4 = Cancelled

BookingActorRole
- 0 = Employee
- 1 = Admin

1. Створити кімнату
```
curl -X POST 'https://localhost:7227/api/rooms' \
  -H 'X-EmployeeID: f8385548-5f79-4f46-a4bb-12e966fed9c2' \
  -H 'X-UserRole: Admin' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "Кімната переговорів",
    "reqCapacity": 7,
    "location": "2-й поверх",
    "isActive": true
  }'

Response 201

{
  "id": 6,
  "name": "Кімната переговорів",
  "reqCapacity": 7,
  "location": "2-й поверх",
  "isActive": true
}
```

2. Створити запит на бронювання
```
curl -X POST 'https://localhost:7227/api/bookings' \
  -H 'Idempotency-Key: e79a6fba-6029-4c98-b3c3-9f85a443a5cd' \
  -H 'X-EmployeeID: 2810d293-e85a-4596-8849-fa33ee2f6ba5' \
  -H 'X-UserRole: Employee' \
  -H 'Content-Type: application/json' \
  -d '{
    "roomId": 6,
    "startedAtUtc": "2026-04-28T12:00:36.438Z",
    "endAtUtc": "2026-04-28T14:00:36.438Z",
    "purpose": "Дуже важлива зустріч!",
    "emails": [
      "1test@gmail.com",
      "2test@gmail.com",
      "3test@gmail.com",
      "4test@gmail.com",
      "5test@gmail.com"
    ]
  }'

 Response 201
{
  "id": 13,
  "roomId": 6,
  "employeeId": "2810d293-e85a-4596-8849-fa33ee2f6ba5",
  "idempotencyKey": "e79a6fba-6029-4c98-b3c3-9f85a443a5cd",
  "startedAtUtc": "2026-04-28T12:00:36.438Z",
  "endAtUtc": "2026-04-28T14:00:36.438Z",
  "purpose": "Дуже важлива зустріч!",
  "status": 0,
  "emails": [
    "1test@gmail.com",
    "2test@gmail.com",
    "3test@gmail.com",
    "4test@gmail.com",
    "5test@gmail.com"
  ]
}
```

3. Відправити бронювання на розгляд
```
curl -X POST 'https://localhost:7227/api/bookings/13/submit' \
  -H 'X-EmployeeID: 2810d293-e85a-4596-8849-fa33ee2f6ba5' \
  -H 'X-UserRole: Employee'
```

5. Підтвердити бронювання
```
curl -X POST 'https://localhost:7227/api/bookings/13/confirm' \
  -H 'X-EmployeeID: 78768278-20d2-4be6-bcdd-dbda3a840634' \
  -H 'X-UserRole: Admin'
```
  7. Отримати бронювання з історією
```
curl -X GET 'https://localhost:7227/api/bookings/13' \
  -H 'X-EmployeeID: e967bc1e-12f4-460a-8409-b55c7bccac2b' \
  -H 'X-UserRole: Employee'
```
9. Пошук бронювань з фільтрами
```
curl -X GET 'https://localhost:7227/api/bookings?roomId=6&from=2026-04-28T12:00:36.438Z&to=2026-04-28T14:00:36.438Z&status=Confirmed' \
  -H 'X-EmployeeID: fafebd04-d287-4810-a7d9-9f7318ae7471' \
  -H 'X-UserRole: Employee'
```
