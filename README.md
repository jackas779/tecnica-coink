# API de Registro de Contactos (Prueba Técnica Coink)

Este proyecto es una API REST construida con **.NET 8** que permite gestionar el registro de contactos. Implementa una arquitectura limpia utilizando el **Patrón Repositorio**, **Dapper** para el acceso a datos de alto rendimiento y **PostgreSQL** como motor de base de datos, haciendo uso de Stored Procedures para la lógica de inserción.

## 📋 Requisitos Previos

Asegúrate de tener instalado lo siguiente en tu entorno local antes de comenzar:

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [PostgreSQL](https://www.postgresql.org/download/) (v12 o superior recomendado)
*   Un cliente SQL (pgAdmin, DBeaver, o terminal)
*   Git
*   [Docker](https://www.docker.com/) (Opcional)

## 🚀 Configuración de la Base de Datos

Puedes configurar la base de datos manualmente o utilizando Docker.

### Opción A: Usando Docker (Recomendado)
Ejecuta el siguiente comando para levantar una instancia de PostgreSQL con la configuración esperada por la API:
```bash
docker run --name coink-db -e POSTGRES_PASSWORD=123456789 -e POSTGRES_DB=coink -p 5432:5432 -d postgres
```

### Opción B: Manualmente
Asegúrate de tener un servidor PostgreSQL corriendo y crea una base de datos llamada `coink`.

---

### Inicialización del Esquema
Independientemente de la opción elegida, conéctate a la base de datos y ejecuta los siguientes scripts:

1.  **Crear tablas paramétricas:**
    Ejecuta el siguiente script para crear y poblar los catálogos de ubicación:

    ```sql
    CREATE TABLE paises (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, nombre VARCHAR(50) NOT NULL);
    CREATE TABLE departamentos (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, nombre VARCHAR(50) NOT NULL, id_pais INT REFERENCES paises(id));
    CREATE TABLE municipios (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, nombre VARCHAR(50) NOT NULL, id_departamento INT REFERENCES departamentos(id));
    
    -- Datos de prueba
    INSERT INTO paises (nombre) VALUES ('Colombia');
    INSERT INTO departamentos (nombre, id_pais) VALUES ('Antioquia', 1), ('Cundinamarca', 1);
    INSERT INTO municipios (nombre, id_departamento) VALUES ('Medellín', 1), ('Bogotá', 2);
    ```

2.  **Crear la tabla de contactos:**
    Ejecuta el siguiente script SQL para crear la estructura de la tabla:

    ```sql
    CREATE TABLE contacts (
        id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        telefono VARCHAR(20) NOT NULL,
        id_pais INT NOT NULL REFERENCES paises(id),
        id_departamento INT NOT NULL REFERENCES departamentos(id),
        id_municipio INT NOT NULL REFERENCES municipios(id),
        direccion VARCHAR(200) NOT NULL,
        fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );
    ```

3.  **Crear el Procedimiento Almacenado:**
    Ejecuta este script para crear la función que maneja la inserción y valida duplicados:

    ```sql
    CREATE OR REPLACE PROCEDURE public.sp_registrar_contacto(
        IN p_nombre VARCHAR(100),
        IN p_telefono VARCHAR(20),
        IN p_id_pais INT,
        IN p_id_departamento INT,
        IN p_id_municipio INT,
        IN p_direccion VARCHAR(200),
        INOUT p_id_generado INT DEFAULT NULL 
    )
    LANGUAGE plpgsql
    AS $$
    BEGIN
        -- Validar si el teléfono ya existe
        IF EXISTS (SELECT 1 FROM public.contactos WHERE telefono = p_telefono) THEN
            RAISE EXCEPTION 'El teléfono ya se encuentra registrado en el sistema.'
            USING ERRCODE = 'P0001';
        END IF;

        -- Insertar y asignar el ID 
        INSERT INTO public.contactos (nombre, telefono, id_pais, id_departamento, id_municipio, direccion)
        VALUES (p_nombre, p_telefono, p_id_pais, p_id_departamento, p_id_municipio, p_direccion)
        RETURNING id INTO p_id_generado;
    END;
    $$;
    ```

## ⚙️ Instalación y Ejecución

1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/jackas779/tecnica-coink.git
    cd coink
    ```

2.  **Configurar la conexión:**
    Abre el archivo `appsettings.json` y actualiza la cadena de conexión `DefaultConnection` con tus credenciales locales de PostgreSQL (Usuario, Contraseña y Puerto).

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=coink;Username=postgres;Password=TU_PASSWORD"
    }
    ```

3.  **Restaurar dependencias y ejecutar:**
    En la terminal, dentro de la carpeta del proyecto:

    ```bash
    dotnet restore
    dotnet run
    ```

4.  **Probar la API:**
    Una vez iniciada, la aplicación mostrará la URL local (ej. `http://localhost:5000`).
    *   Puedes acceder a **Swagger UI** en: `http://localhost:<PUERTO>/swagger`
    *   Usa el endpoint `POST /api/contactos` para enviar datos de prueba.

5.  **Ejemplo Json de Prueba**
    ```json
    {
      "nombre": "Juan Perez",
      "telefono": "3001234567",
      "idPais": 1,
      "idDepartamento": 1,
      "idMunicipio": 1,
      "direccion": "Calle Falsa 123"
    }
    ```