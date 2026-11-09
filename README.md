# Parcial #3 – Herramientas de la Programación Aplicada III (.NET)

**Universidad Tecnológica de Panamá** – Facultad de Ingeniería en Sistemas
**Grupo:** 1IL133 · **Instructora:** Ing. Irina Fong
**Estudiante:** Norlan Ortega
**Semestre:** II Semestre 2026

## Contenido del repositorio

```
Parcial_3/
├── Figuras/            # Parte 1: jerarquía de clases y polimorfismo
├── ejemploJson/        # Parte 2: JSON con Newtonsoft.Json
├── LoginBCrypt/        # Parte 3: login con BCrypt y SQL Server
│   └── BaseDatos.sql   # Script de creación de la BD
├── LoginBCryptDB.bak   # Backup de la base de datos
└── capturas/           # Evidencias de ejecución
```

## Requisitos

- .NET SDK (proyectos de consola en C#)
- SQL Server y SQL Server Management Studio (SSMS)
- Paquetes NuGet:
  - `Newtonsoft.Json` (Parte 2)
  - `BCrypt.Net-Next` y `Microsoft.Data.SqlClient` (Parte 3)

---

## Parte 1: Jerarquía de figuras (polimorfismo)

Proyecto: `Figuras`

- Clase abstracta `Figura` con los métodos abstractos `CalcularArea()` y `CalcularPerimetro()`.
- Clases derivadas: `Rectangulo`, `Cuadrado` y `Triangulo` (el área del triángulo se calcula con la fórmula de Herón).
- Un arreglo unidimensional `Figura[]` que almacena objetos de las tres clases hijas.
- Un `foreach` que recorre el arreglo y muestra el nombre de la clase, el área y el perímetro de cada figura.

**Ejecución:**

```
cd Figuras
dotnet run
```

<img width="1749" height="615" alt="image" src="https://github.com/user-attachments/assets/729f54c9-096d-4e05-b2ad-0f77d5a973a5" />


---

## Parte 2: Script JSON con Newtonsoft.Json

Proyecto: `ejemploJson`

Crea un objeto con los datos de una persona (Nombre, Edad, Carrera) y lo convierte a una cadena JSON con `JsonConvert.SerializeObject`.

**Ejecución:**

```
cd ejemploJson
dotnet add package Newtonsoft.Json
dotnet run
```

<img width="1828" height="543" alt="image" src="https://github.com/user-attachments/assets/05bc3153-d0a0-4509-83db-95ccd2aa6753" />


---

## Parte 3: Login con BCrypt y SQL Server

Proyecto: `LoginBCrypt`

### Base de datos

Base de datos `LoginBCryptDB`, creada con `LoginBCrypt/BaseDatos.sql`:

| Tabla | Descripción |
|-------|-------------|
| `Usuarios` | Id, NombreUsuario (único), NombreCompleto, PasswordHash, FechaRegistro |
| `IntentosLogin` | Id, NombreUsuario, Exitoso (1 = exitoso, 0 = fallido), Fecha. Sirve para análisis posterior |

La contraseña nunca se guarda en texto plano: solo se almacena el hash BCrypt.

### Funcionamiento

1. **Registrar usuario:** se genera el hash con `BCrypt.Net.BCrypt.HashPassword()` y se guarda en `Usuarios`.
2. **Iniciar sesión:** se compara la contraseña ingresada con el hash guardado usando `BCrypt.Net.BCrypt.Verify()`. Si es correcta, se muestra el nombre del usuario.
3. Cada intento, exitoso o fallido, se registra en `IntentosLogin` con el usuario y la fecha.
4. **Ver historial de intentos:** muestra los registros de `IntentosLogin`.

### Cómo ejecutarlo

1. Restaurar el backup `LoginBCryptDB.bak` en SQL Server (clic derecho en *Databases* → *Restore Database...*), o ejecutar `LoginBCrypt/BaseDatos.sql`.
2. Ajustar la cadena de conexión `CadenaConexion` en `Program.cs` según el nombre del servidor.
3. Ejecutar:

```
cd LoginBCrypt
dotnet restore
dotnet run
```

### Evidencias

**Registro de usuario y acceso exitoso (muestra el nombre del usuario):**

<img width="562" height="311" alt="Screenshot 2026-11-09 103652" src="https://github.com/user-attachments/assets/455df2f3-5d0b-4f27-a932-51ad905102ce" />


**Historial de intentos desde la aplicación:**

<img width="462" height="239" alt="Screenshot 2026-11-09 103658" src="https://github.com/user-attachments/assets/54a9d4c0-70b2-4c5a-9aa4-38f4eb9212d0" />


**Datos guardados en SQL Server (`Usuarios` con el hash e `IntentosLogin`):**

<img width="1263" height="844" alt="image" src="https://github.com/user-attachments/assets/e22ea845-31fa-4c1d-81be-ef6b7d27ab4e" />


---

## Resumen de puntos

| Parte | Puntos |
|-------|--------|
| 1. Figuras con clase abstracta y polimorfismo | 15 |
| 2. Script JSON | 15 |
| 3. Hash y login con BCrypt | 20 |
