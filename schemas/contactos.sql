CREATE TABLE contactos (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    id_pais INT NOT NULL REFERENCES paises(id),
    id_departamento INT NOT NULL REFERENCES departamentos(id),
    id_municipio INT NOT NULL REFERENCES municipios(id),
    direccion VARCHAR(200) NOT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
