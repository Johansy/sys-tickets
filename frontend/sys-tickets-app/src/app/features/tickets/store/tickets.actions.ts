import { createAction, props} from '@ngrx/store';
import { TicketDetalle, TicketFiltros, TicketListItem, Usuario, Categoria } from '../../../core/models';

//Listado
export const cargarTickets = createAction(
    '[Tickets] Cargar Tickets',
    props<{ filtros: TicketFiltros }>()
);
export const cargarTicketsSuccess = createAction(
    '[Tickets] Cargar Tickets Success',
    props<{ tickets: TicketListItem[], totalRegistros: number, totalPaginas: number }>()
);
export const cargarTicketsFailure = createAction(
    '[Tickets] Cargar Tickets Failure',
    props<{ error: string }>()
);
export const actualizarFiltros = createAction(
    '[Tickets] Actualizar Filtros',
    props<{ filtros: Partial<TicketFiltros> }>()
);

//Detalle
export const cargarDetalle = createAction(
    '[Tickets] Cargar Detalle',
    props<{ id: number }>()
);
export const cargarDetalleSuccess = createAction(
    '[Tickets] Cargar Detalle Success',
    props<{ ticket: TicketDetalle }>()
);
export const cargarDetalleFailure = createAction(
    '[Tickets] Cargar Detalle Failure',
    props<{ error: string }>()
);

//Camiar estatus
export const cambiarEstatus = createAction(
    '[Tickets] Cambiar Estatus',
    props<{ id: number, estatus: string }>()
);
export const cambiarEstatusSuccess = createAction(
    '[Tickets] Cambiar Estatus Success',
    props<{ id: number, estatus: string }>()
);  
export const cambiarEstatusFailure = createAction(
    '[Tickets] Cambiar Estatus Failure',
    props<{ error: string }>()
);

//Asignar agente
export const asignarAgente = createAction(
    '[Tickets] Asignar Agente',
    props<{ id: number, agenteId: number }>()
);
export const asignarAgenteSuccess = createAction('[Ticket Detail] Assign Agent Success');
export const asignarAgenteFailure = createAction(
  '[Ticket Detail] Assign Agent Failure',
  props<{ error: string }>()
);

//Agregar comentario
export const agregarComentario = createAction(
    '[Tickets] Agregar Comentario',
    props<{ id: number, contenido: string, esInterno: boolean }>()
);

export const agregarComentarioSuccess = createAction('[Tickets] Agregar Comentario Success');
export const agregarComentarioFailure = createAction(
  '[Tickets] Agregar Comentario Failure',
  props<{ error: string }>()
);

//Crear ticket
export const crearTicket = createAction(
    '[Tickets] Crear Ticket',
    props<{ titulo: string, descripcion: string, categoriaId: number, prioridad: string }>()
);
export const crearTicketSuccess = createAction(
    '[Tickets] Crear Ticket Success',
    props<{ id: number }>()
);      
export const crearTicketFailure = createAction(
    '[Tickets] Crear Ticket Failure',
    props<{ error: string }>()
);

//Cargar catálogos
export const cargarCatalogos = createAction('[Catalogos] Cargar Catalogos');
export const cargarCatalogosSuccess = createAction(
    '[Catalogos] Cargar Catalogos Success',
    props<{ categorias: Categoria[], agentes: Usuario[] }>()
);