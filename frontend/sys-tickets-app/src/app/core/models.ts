// Interfaces compartidad (DTOs, modelos) entre frontend y backend

export type Prioridad = 'Baja' | 'Media' | 'Alta' | 'Urgente';
export type Estatus = 'abierto' | 'en_progreso' | 'resuelto' | 'cerrado';

export interface TicketListItem {
    id: number; 
    titulo: string;
    categoriaId: number;
    categoriaNombre: string;
    prioridad: Prioridad;
    estatus: Estatus;
    creadorNombre: string;
    fechaCreacion: string;
    agenteNombre?: string;
    totalComentarios: number;
}

export interface Comentario {
    id: number;
    ticketId: number;
    autorId: number;
    autorNombre: string;
    contenido: string;
    esInterno: boolean;
    fecha: string;
}

export interface HistorialItem {
    id: number;
    ticketId: number;
    campoModificado: string;
    valorAnterior?: string;
    valorNuevo?: string;
    modificadoPorNombre?: string;
    fechaModificacion: string;
}

export interface TicketDetalle {
    id: number;
    titulo: string;
    descripcion: string;
    categoriaId: number;
    categoriaNombre: string;
    prioridad: Prioridad;
    estatus: Estatus;
    creadoPor: number;
    creadorNombre: string;
    asignadoA?: number;
    agenteNombre?: string;
    fechaCreacion: string;
    fechaResolucion?: string;
    comentarios: Comentario[];
    historial: HistorialItem[];
}

export interface PaginateResult<T> {
    items: T[];
    totalRegistros: number;
    paginas: number;
    registrosPorPagina: number;
    totalPaginas: number;
}

export interface Categoria {
    id: number;
    nombre: string;
}

export interface Usuario {
    id: number;
    nombre: string;
    email: string;
    rol: string;
}

export interface TicketFiltros {
    texto?: string;
    categoriaId?: number;
    prioridad?: string;
    estatus?: string;
    agenteId?: number;
    pagina: number;
    registros: number;
}