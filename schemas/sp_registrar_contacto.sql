CREATE OR REPLACE FUNCTION sp_registrar_contacto(
    p_nombre VARCHAR(100),
    p_telefono VARCHAR(20),
    p_pais VARCHAR(50),
    p_departamento VARCHAR(50),
    p_municipio VARCHAR(50),
    p_direccion VARCHAR(200)
)
RETURNS INT AS $$
DECLARE
    v_id_generado INT;
BEGIN
    -- Validar si el teléfono ya existe
    IF EXISTS (SELECT 1 FROM contactos WHERE telefono = p_telefono) THEN
        RAISE EXCEPTION 'El teléfono ya se encuentra registrado en el sistema.'
        USING ERRCODE = 'P0001'; -- Código de error personalizado (User Defined)
    END IF;

    -- Insertar y retornar el ID generado inmediatamente
    INSERT INTO contactos (nombre, telefono, pais, departamento, municipio, direccion)
    VALUES (p_nombre, p_telefono, p_pais, p_departamento, p_municipio, p_direccion)
    RETURNING id INTO v_id_generado;

    RETURN v_id_generado;
END;
$$ LANGUAGE plpgsql;