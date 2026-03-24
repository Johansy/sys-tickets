--SCHEMA: Crea un esquema general para la base de datos.

CREATE EXTENSION IF NOT EXISTS pg_trgm; -- Habilita la extensión pg_trgm para mejorar las búsquedas de texto.

--Usuarios: (Rol de Agente|Usuario)
CREATE TABLE IF NOT EXISTS usuarios (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    rol VARCHAR(20) NOT NULL CHECK (rol IN ('Agente', 'Usuario'))
);

--Categorías: (facturación, soporte técnico, accesos,otro).
CREATE TABLE IF NOT EXISTS categorias (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(20) NOT NULL
);

--Tickets
CREATE TABLE IF NOT EXISTS tickets (
    id SERIAL PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    descripcion TEXT NOT NULL,
    estatus VARCHAR(20) NOT NULL CHECK (estatus IN ('abierto', 'en_progreso', 'resuelto', 'cerrado')),
    prioridad VARCHAR(20) NOT NULL CHECK (prioridad IN ('baja', 'media', 'alta', 'urgente')),
    fecha_creacion TIMESTAMP NOT NULL DEFAULT NOW(),
    fecha_resolucion TIMESTAMP,
    categoria_id INT NOT NULL REFERENCES categorias(id),
    creado_por INT NOT NULL REFERENCES usuarios(id),
    asignado_a INT REFERENCES usuarios(id)
);

--Comentarios
CREATE TABLE IF NOT EXISTS comentarios (
    id SERIAL PRIMARY KEY,
    contenido TEXT NOT NULL,
    fecha TIMESTAMP NOT NULL DEFAULT NOW(),
    ticket_id INT NOT NULL REFERENCES tickets(id) ON DELETE CASCADE,
    autor_id INT NOT NULL REFERENCES usuarios(id),
    es_interno BOOLEAN NOT NULL DEFAULT FALSE
);

--Historial de Tickets
CREATE TABLE IF NOT EXISTS ticket_historial (
    id SERIAL PRIMARY KEY,
    ticket_id INT NOT NULL REFERENCES tickets(id) ON DELETE CASCADE,
    modificado_por INT REFERENCES usuarios(id),
    campo_modificado VARCHAR(50) NOT NULL,
    valor_anterior VARCHAR(200),
    valor_nuevo VARCHAR(200) NOT NULL,
    fecha_cambio TIMESTAMP NOT NULL DEFAULT NOW()
);