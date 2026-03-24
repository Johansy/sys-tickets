--FUNCIÓN: Lógica con función PL/pgSQL para buscar tickets con paginación.

CREATE OR REPLACE FUNCTION buscar_tickets(
    p_texto VARCHAR DEFAULT NULL,
    p_categoria_id INT DEFAULT NULL,
    p_prioridad VARCHAR DEFAULT NULL,
    p_estatus VARCHAR DEFAULT NULL,
    p_agente_id INT DEFAULT NULL,
    p_pagina INT DEFAULT 1,
    p_registros INT DEFAULT 10
) 
RETURNS TABLE (
    ticket_id INT,
    titulo VARCHAR,
    descripcion TEXT,
    estatus VARCHAR,
    prioridad VARCHAR,
    fecha_creacion TIMESTAMP,
    fecha_resolucion TIMESTAMP,
    categoria_id INT,
    categoria_nombre VARCHAR,
    asignado_a_id INT,
    agente_nombre VARCHAR,
    creador_nombre VARCHAR,
    total_comentarios BIGINT,
    total_registros BIGINT
) AS $$
DECLARE
    v_offset INT := (p_pagina - 1) * p_registros;
BEGIN
    RETURN QUERY
    SELECT 
        t.id AS ticket_id,
        t.titulo,
        t.descripcion,
        t.estatus,
        t.prioridad,
        t.fecha_creacion,
        t.fecha_resolucion,
        c.id AS categoria_id,
        c.nombre AS categoria_nombre,
        a.id AS asignado_a_id,
        a.nombre AS agente_nombre,
        u.nombre AS creador_nombre,
        COUNT(com.id) AS total_comentarios,
        COUNT(*) OVER() AS total_registros
    FROM tickets t
    JOIN categorias c ON t.categoria_id = c.id
    JOIN usuarios u ON u.id = t.creado_por
    LEFT JOIN usuarios a ON a.id = t.asignado_a
    LEFT JOIN comentarios com ON com.ticket_id = t.id
    WHERE 
        (p_texto IS NULL OR t.titulo ILIKE '%' || p_texto || '%') AND
        (p_categoria_id IS NULL OR t.categoria_id = p_categoria_id) AND
        (p_estatus IS NULL OR t.estatus = p_estatus) AND
        (p_prioridad IS NULL OR t.prioridad = p_prioridad) AND
        (p_agente_id IS NULL OR t.asignado_a = p_agente_id)
    GROUP BY t.id, c.id, u.id, a.id
    ORDER BY t.fecha_creacion DESC
    LIMIT p_registros OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;