CREATE TABLE paises (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL
);

CREATE TABLE departamentos (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    id_pais INT NOT NULL REFERENCES paises(id)
);

CREATE TABLE municipios (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    id_departamento INT NOT NULL REFERENCES departamentos(id)
);

-- Datos semilla (Seed data) para pruebas
INSERT INTO paises (nombre) VALUES ('Colombia');
INSERT INTO departamentos (nombre, id_pais) VALUES ('Antioquia', 1), ('Cundinamarca', 1);
INSERT INTO municipios (nombre, id_departamento) VALUES ('Medellín', 1), ('Bogotá', 2);