RecipeHub - REST API для керування кулінарними рецептами

RecipeHub — це вебзастосунок (RESTful API), розроблений на базі ASP.NET Core, який дозволяє користувачам створювати, переглядати, оновлювати та видаляти кулінарні рецепти, керувати категоріями, інгредієнтами та зображеннями, а також забезпечує реєстрацію, автентифікацію та розмежування прав доступу за допомогою JWT (JSON Web Tokens).

Репозиторій проєкту: https://github.com/chainmeJB/recipe-hub

Деплой (Render + Swagger UI): https://recipe-hub-kyol.onrender.com/swagger


## Технологічний стек

* Платформа та фреймворк: .NET 8 / ASP.NET Core Web API
* База даних: PostgreSQL (хмарний хостинг Supabase)
* ORM та міграції: Entity Framework Core (Code First, Npgsql)
* Автентифікація та безпека: JWT Bearer Authentication, хешування паролів за допомогою BCrypt.Net-Next
* Мапінг та валідація: AutoMapper, FluentValidation
* Документація API: Swagger / OpenAPI (Swashbuckle)
* Деплой: Render (Web Service) + Supabase (PostgreSQL)

## Архітектура проєкту

Проєкт побудований за принципами багатошарової архітектури (Layered Architecture) із розділенням на три основні проєкти:

* `RecipeHub.Api` — рівень представлення:
  * REST-контролери (AuthController, RecipeController, CategoryController, IngredientController, UserController тощо)
  * Вхідні/вихідні моделі запитів і відповідей (Request_Models, Response_Models)
  * Профілі мапінгу API (APIMappingProfile)
  * Глобальна обробка винятків (ExceptionHandlingMiddleware)
  * Налаштування Dependency Injection (DI), CORS, Swagger та JWT Bearer у Program.cs
* `RecipeHub.Application` — рівень бізнес-логіки:
  * Сервіси (AuthService, UserService, RecipeService, CategoryService, IngredientService)
  * Інтерфейси сервісів та генератора токенів (IAuthService, IUserService, IJwtTokenGenerator тощо)
  * Об'єкти передачі даних (DTO) та перерахування (UserRole)
  * Валідатори (FluentValidation) та профілі мапінгу бізнес-логіки (MappingProfile)
  * Генерація та підпис JWT-токенів (JwtTokenGenerator, JwtOptions)
* `RecipeHub.Infrastructure` — рівень доступу до даних:
  * Контекст бази даних (RecipeHubDbContext)
  * Сутності бази даних (User, Recipe, Category, Ingredient, RecipeIngredient, Image)
  * Конфігурації сутностей EF Core (Fluent API) та міграції бази даних
  * Узагальнений та спеціалізовані репозиторії (IRepository<T>, Repository<T>)

## Автентифікація та рольова модель

У системі використовується автентифікація на основі JWT Bearer-токенів. Після успішного входу або реєстрації клієнт отримує accessToken, який передається у заголовку Authorization: Bearer <token>.

### Клейми (Claims) у JWT-токені
* ClaimTypes.NameIdentifier — унікальний ідентифікатор користувача (Id)
* ClaimTypes.Email — електронна пошта користувача
* ClaimTypes.Name — відображуване ім'я користувача (DisplayName)
* ClaimTypes.Role — роль користувача в системі (User або Admin)

### Ролі та матриця прав доступу

* Гість (Неавторизований користувач — без токена):
  * Реєстрація нового облікового запису (POST /api/Auth/register)
  * Вхід у систему та отримання токена (POST /api/Auth/login)
  * Перегляд списку всіх рецептів та деталей конкретного рецепта (GET)
  * Перегляд списку всіх категорій та інгредієнтів (GET)

* User (Авторизований звичайний користувач):
  * Усі можливості Гостя
  * Перегляд власного профілю (GET /api/Auth/me)
  * Створення нових рецептів (POST), де авторство автоматично прив'язується до поточного користувача
  * Редагування (PUT) та видалення (DELETE) виключно власних рецептів (спроба змінити чужий рецепт повертає 403 Forbidden)
  * Додавання/керування зображеннями власних рецептів
* Admin (Адміністратор системи):
  * Усі можливості ролі User
  * Повне керування довідником категорій: створення (POST), редагування (PUT) та видалення (DELETE)
  * Повне керування довідником інгредієнтів: створення (POST), редагування (PUT) та видалення (DELETE)
  * Редагування та видалення будь-яких рецептів у системі (модерація контенту)
  * Перегляд списку користувачів, зміна їхніх ролей (User, Admin) та видалення облікових записів


## Детальний перелік ендпоінтів API

### Автентифікація та користувач (/api/Auth)
* `POST /api/Auth/register` *(Public)*
  * Реєструє нового користувача, хешує пароль за допомогою BCrypt та одразу повертає JWT-токен і дані профілю (200 OK / 201 Created).
  * Тіло запиту: email, password, displayName.
* `POST /api/Auth/login` *(Public)*
  * Перевіряє email та пароль користувача, генерує JWT-токен (200 OK) або повертає 401 Unauthorized при невірних облікових даних.
  * Тіло запиту: email, password.
* `GET /api/Auth/me` *(Authorized: User, Admin)*
  * Зчитує ідентифікатор користувача з клеймів JWT-токена та повертає актуальні дані поточного профілю (Id, Email, DisplayName, Role, CreatedAt).

### Рецепти (/api/Recipe або /api/Recipes)
* `GET /api/Recipe` *(Public)*
  * Повертає список усіх рецептів (із підтримкою фільтрації за категорією, пошуку за назвою або пагінації).
* `GET /api/Recipe/{id}` *(Public)*
  * Повертає повну інформацію про рецепт за його id: опис, покрокову інструкцію, час підготовки та приготування, кількість порцій, категорію, автора та список інгредієнтів із кількістю.
* `POST /api/Recipe` *(Authorized: User, Admin)*
  * Створює новий рецепт від імені поточного авторизованого користувача.
* `PUT /api/Recipe/{id}` *(Authorized: Власник рецепта або Admin)*
  * Оновлює дані існуючого рецепта та його склад інгредієнтів. Повертає 403 Forbidden, якщо запит робить не автор і не адміністратор.
* `DELETE /api/Recipe/{id}` *(Authorized: Власник рецепта або Admin)*
  * Видаляє рецепт із бази даних.

### Категорії (/api/Category або /api/Categories)
* `GET /api/Category` *(Public)*
  * Повертає список усіх доступних категорій страв.
* `GET /api/Category/{id}` *(Public)*
  * Повертає інформацію про конкретну категорію за id.
* `POST /api/Category` *(Authorized: Admin)*
  * Створює нову категорію страв (name, description).
* `PUT /api/Category/{id}` *(Authorized: Admin)*
  * Оновлює назву або опис категорії.
* `DELETE /api/Category/{id}` *(Authorized: Admin)*
  * Видаляє категорію.

### Інгредієнти (/api/Ingredient або /api/Ingredients)
* `GET /api/Ingredient` *(Public)*
  * Повертає довідник усіх інгредієнтів.
* `GET /api/Ingredient/{id}` *(Public)*
  * Повертає інформацію про інгредієнт за id.
* `POST /api/Ingredient` *(Authorized: Admin)*
  * Додає новий інгредієнт до загального довідника.
* `PUT /api/Ingredient/{id}` *(Authorized: Admin)*
  * Оновлює дані інгредієнта.
* `DELETE /api/Ingredient/{id}` *(Authorized: Admin)*
  * Видаляє інгредієнт.

### Керування користувачами (/api/User / /api/Users)
* `GET /api/User` *(Authorized: Admin)*
  * Повертає перелік усіх зареєстрованих користувачів.
* `GET /api/User/{id}` *(Authorized: Admin або сам користувач)*
  * Повертає профіль користувача за id.
* `PATCH / PUT /api/User/{id}/role` *(Authorized: Admin)*
  * Змінює роль користувача (User / Admin).
* `DELETE /api/User/{id}` *(Authorized: Admin)*
  * Видаляє обліковий запис користувача.

---

## Схема бази даних (PostgreSQL)

* `Users`: Id (PK), Email (Unique), PasswordHash, DisplayName, Role, CreatedAt
* `Categories`: Id (PK), Name, Description
* `Recipes`: Id (PK), Title, Description, Instructions, PrepTimeMinutes, CookTimeMinutes, Servings, CategoryId (FK → Categories), AuthorId (FK → Users), CreatedAt
* `Ingredients`: Id (PK), Name
* `RecipeIngredients`: RecipeId (PK, FK → Recipes), IngredientId (PK, FK → Ingredients), Quantity, Unit
* `Images`: Id (PK), Url, RecipeId (FK → Recipes)

---

## Обробка помилок та HTTP-статуси

API використовує стандартні коди відповідей HTTP та централізований ExceptionHandlingMiddleware:
* `200 OK` / `201 Created` — запит виконано успішно
* `204 No Content` — успішне оновлення або видалення ресурсу
* `400 Bad Request` / `422 Unprocessable Entity` — помилка валідації вхідних даних (FluentValidation) або некоректні параметри запиту
* `401 Unauthorized` — відсутній, прострочений або невалідний JWT-токен, або невірний email/пароль при вході
* `403 Forbidden` — токен валідний, але у користувача недостатньо прав (наприклад, звичайний User намагається змінити чужий рецепт або створити категорію)
* `404 Not Found` — запитуваний ресурс (рецепт, категорію, інгредієнт чи користувача) не знайдено
* `409 Conflict` — спроба реєстрації з email, який уже існує в базі даних
* `500 Internal Server Error` — непередбачена внутрішня помилка сервера

---

## Локальний запуск та налаштування

### 1. Вимоги
* [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
* PostgreSQL 15+ (локальний сервер або хмарна база даних Supabase)
* *(Опціонально)* [Docker](https://www.docker.com/) — для запуску в контейнері

### 2. Клонування репозиторію
git clone [https://github.com/chainmeJB/recipe-hub.git](https://github.com/chainmeJB/recipe-hub.git)
cd recipe-hub


## Деплой (Deployment)

Проєкт розгорнуто у хмарному середовищі за допомогою **Render** (Web Service на базі Docker) та **Supabase** (керована база даних PostgreSQL).

### Покрокова інструкція з розгортання:

1. **Підготовка бази даних (Supabase):**
   * Створено проєкт у Supabase та отримано рядок підключення (Connection String) до PostgreSQL.
   * Попередньо створено початкові міграції Entity Framework Core (`dotnet ef migrations add InitialCreate`), які відповідають за ініціалізацію структури таблиць.

2. **Конфігурація Docker:**
   * Підготовлено багатоетапний `Dockerfile` для збірки та запуску застосунку на базі .NET 8.
   * Додано автоматичне застосування міграцій EF Core при старті контейнера (або під час збірки), щоб схема бази даних у Supabase завжди залишалася актуальною без ручного втручання.

3. **Налаштування сервісу на Render:**
   * Створено новий **Web Service** на Render із прив'язкою до GitHub-репозиторію та обраним середовищем **Docker**.
   * У розділі **Environment / Secret Files** додано необхідні змінні середовища:
     * `ConnectionStrings__DefaultConnection` — рядок підключення до PostgreSQL (Supabase).
     * `Jwt__Key` — секретний ключ для підпису JWT-токенів.
     * `Jwt__Issuer` та `Jwt__Audience` — параметри емітента та аудиторії токена.
     * `ASPNETCORE_ENVIRONMENT=Production` (або увімкнено Swagger для публічного доступу до документації).
   * Після пушу в основну гілку Render автоматично збирає Docker-образ, застосовує міграції та піднімає API.

## Виконані додаткові завдання (Side Quests & Extensions)

Окрім базового CRUD-функціоналу, у проєкті реалізовано низку додаткових архітектурних та інфраструктурних рішень:

* **Role-Based Access Control (RBAC):** Повноцінне розмежування прав доступу на рівні контролерів та сервісів для трьох категорій користувачів (`Гість`, `User`, `Admin`) із перевіркою авторства контенту (користувач може редагувати й видаляти лише власні рецепти, тоді як адміністратор має права модерації та керування довідниками).
* **CI/CD та контейнеризація:** Налаштовано автоматичний пайплайн збірки та деплою через Docker на Render при оновленні коду в репозиторії, включно з автоматичним накатуванням міграцій БД.
* **Глобальна обробка помилок:** Реалізовано власний `ExceptionHandlingMiddleware`, який перехоплює винятки та повертає стандартизовані HTTP-відповіді (Problem Details).
* **Валідація та мапінг:** Інтегровано `FluentValidation` для суворої перевірки вхідних DTO та `AutoMapper` для чистого перетворення моделей між шарами `Infrastructure`, `Application` та `Api`.
* **Робота із зображеннями рецептів:** Підтримка прив'язки зображень до страв через окрему сутність `Images` (зв'язок `1:N`).

---

## Відомі обмеження та плани розвитку (Known Limitations & TODOs)

* **«Холодний старт» на Render:** Оскільки API розміщено на безкоштовному тарифі Render (Free Instance), сервер переходить у сплячий режим після 15 хвилин неактивності. Через це перший запит після простою може виконуватися **30–50 секунд** (наступні запити працюють миттєво).
* **Лише Access-токени (відсутність Refresh Tokens):** Наразі використовується стандартний JWT Access Token без механізму ротації через Refresh-токени та без чорного списку (blacklist) для примусового виходу з усіх пристроїв.
* **Зберігання зображень за URL:** Поточна версія зберігає посилання на зовнішні зображення у базі даних. У майбутньому планується інтеграція з хмарним сховищем (Supabase Storage або AWS S3) для прямого завантаження файлів (`multipart/form-data`).
* **Покриття тестами:** Проєкт має чисту шарувату архітектуру з використанням інтерфейсів та DI, що робить його готовим до тестування, проте написання Unit- та інтеграційних тестів (xUnit + Testcontainers) винесено в наступну ітерацію.
* **Розширений пошук та кешування:** Планується додати повнотекстовий пошук за списком інгредієнтів (пошук страв за тим, що є в холодильнику) та кешування довідників категорій/інгредієнтів.

## Команда та розподіл обов'язків (Team)
* **Юлія Бенедюк: https://github.com/chainmeJB** — **Інфраструктура, безпека та деплой:**
  * Проєктування та реалізація шару `RecipeHub.Infrastructure` (контекст бази даних `RecipeHubDbContext`, моделі сутностей, репозиторії та початкові міграції EF Core).
  * Реалізація 4 сервісів бізнес-логіки (включно з автентифікацією та генерацією/валідацією JWT-токенів) та 4 відповідних REST-контролерів.
  * Налаштування Docker-контейнеризації, підключення хмарної БД (Supabase) та деплой застосунку на Render.

* **Катерина Авласова: https://github.com/demixjr** — **Контракти даних, валідація та бізнес-логіка:**
  * Проєктування моделей передачі даних (DTO) на рівні `RecipeHub.Application` та вхідних/вихідних контрактів (`Request_Models`, `Response_Models`) на рівні `RecipeHub.Api`.
  * CI/CD пайплайн
  * Налаштування профілів `AutoMapper` для обох рівнів (`APIMappingProfile` та `MappingProfile`).
  * Реалізація правил валідації вхідних запитів за допомогою `FluentValidation`.
  * Розробка інтерфейсів для сервісів, імплементація 3 сервісів бізнес-логіки та 3 відповідних REST-контролерів.

* **Спільна робота:** Проєктування архітектури, крос-рев'ю коду, інтеграція шарів, тестування ендпоінтів та спільне вирішення критичних багів і хотфіксів (hotfixes).
