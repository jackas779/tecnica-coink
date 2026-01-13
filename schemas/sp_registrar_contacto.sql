CREATE OR REPLACE PROCEDURE public.sp_registrar_contacto(
    IN p_nombre VARCHAR(100),
    IN p_telefono VARCHAR(20),
    IN p_pais VARCHAR(50),
    IN p_departamento VARCHAR(50),
    IN p_municipio VARCHAR(50),
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
    INSERT INTO public.contactos (nombre, telefono, pais, departamento, municipio, direccion)
    VALUES (p_nombre, p_telefono, p_pais, p_departamento, p_municipio, p_direccion)
    RETURNING id INTO p_id_generado;
END;
$$;