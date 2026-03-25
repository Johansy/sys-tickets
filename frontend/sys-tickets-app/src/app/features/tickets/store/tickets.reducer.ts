import { createReducer, on } from "@ngrx/store";
import { TicketsState, initialState } from "./tickets.state";
import * as TicketsActions from "./tickets.actions";

export const ticketsReducer = createReducer(
    initialState,
    
    
//Listado
on(TicketsActions.cargarTickets, (state, { filtros }) => ({
    ...state, filtros, loadingList: true, errorList: null
})),
on(TicketsActions.cargarTicketsSuccess, (state, { tickets, totalRegistros, totalPaginas }) => ({
    ...state, tickets, totalRegistros, totalPaginas, loadingList: false
})),
on(TicketsActions.cargarTicketsFailure, (state, { error }) => ({
    ...state, loadingList: false, errorList: error
})),
on(TicketsActions.actualizarFiltros, (state, { filtros }) => ({
    ...state, filtros: { ...state.filtros, ...filtros }
})),

//Detalle
on(TicketsActions.cargarDetalle, (state) => ({
    ...state, loadingDetalle: true, errorDetalle: null, ticketSeleccionado: null
})),
on(TicketsActions.cargarDetalleSuccess, (state, { ticket }) => ({
    ...state, ticketSeleccionado: ticket, loadingDetalle: false
})),
on(TicketsActions.cargarDetalleFailure, (state, { error }) => ({
    ...state, loadingDetalle: false, errorDetalle: error
})),

//Cambiar estatus
on(TicketsActions.cambiarEstatus, (state) => ({
    ...state, creando: true, errorCrear: null, errorDetalle: null
})),
on(TicketsActions.cambiarEstatusSuccess, state => ({
    ...state, creando: false            
})),
on(TicketsActions.cambiarEstatusFailure, (state, { error }) => ({
    ...state, creando: false, errorDetalle: error
})),

on(TicketsActions.asignarAgente, (state) => ({
    ...state, errorDetalle: null
})),
on(TicketsActions.asignarAgenteFailure, (state, { error }) => ({
    ...state, errorDetalle: error
})),

on(TicketsActions.agregarComentario, (state) => ({
    ...state, errorDetalle: null
})),
on(TicketsActions.agregarComentarioFailure, (state, { error }) => ({
    ...state, errorDetalle: error
})),

//Catalogos
on(TicketsActions.cargarCatalogosSuccess, (state, { categorias, agentes }) => ({
    ...state, categorias, agentes
}))
);