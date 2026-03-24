/*--ÍNDICES: Lógica para crear índices en las tablas 
relevantes para mejorar el rendimiento de las consultas.--*/

--Índice para Agente y Estatus (filtro principal del dashboard)
CREATE INDEX IF NOT EXISTS idx_tickets_agente_estatus
    ON tickets (asignado_a, estatus);

--Índice para Categorías en el listado (segundo filtro más usado)
CREATE INDEX IF NOT EXISTS idx_tickets_categoria
    ON tickets (categoria_id);


--Índice para Prioridad
CREATE  INDEX IF NOT EXISTS idx_tickets_prioridad
    ON tickets (prioridad);  


--Índice para fecha de creación (orden descendente)
CREATE INDEX IF NOT EXISTS idx_tickets_fecha_creacion
    ON tickets (fecha_creacion DESC);      


--Índice para búsquedas por título con ILIKE (usando pg_trgm para mejorar el rendimiento de las búsquedas de texto)
CREATE INDEX IF NOT EXISTS idx_tickets_titulo_trgm
    ON tickets USING gin (titulo gin_trgm_ops);    


--Comentarios en orden cronológico.
CREATE INDEX IF NOT EXISTS idx_comentarios_ticket_fecha
    ON comentarios (ticket_id, fecha ASC);


--Historial de cambios por ticket en orden cronológico inverso.
CREATE INDEX IF NOT EXISTS idx_historial_ticket_fecha
    ON ticket_historial (ticket_id, fecha_cambio DESC);    