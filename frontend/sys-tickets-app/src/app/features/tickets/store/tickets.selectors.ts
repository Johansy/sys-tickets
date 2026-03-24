import { createFeatureSelector, createSelector } from "@ngrx/store";
import { TicketsState } from "./tickets.state";

export const selectTicketsState = createFeatureSelector<TicketsState>('tickets');

export const selectTickets = createSelector(selectTicketsState, s => s?.tickets ?? []);
export const selectFiltros = createSelector(selectTicketsState, s => s?.filtros ?? { pagina: 1, registros: 10 });
export const selectTotalRegistros = createSelector(selectTicketsState, s => s?.totalRegistros ?? 0);
export const selectTotalPaginas = createSelector(selectTicketsState, s => s?.totalPaginas ?? 0);
export const selectLoadingList = createSelector(selectTicketsState, s => s?.loadingList ?? false);
export const selectErrorList = createSelector(selectTicketsState, s => s?.errorList ?? null);

export const selectTicketSeleccionado = createSelector(selectTicketsState, s => s?.ticketSeleccionado ?? null);
export const selectLoadingDetalle = createSelector(selectTicketsState, s => s?.loadingDetalle ?? false);
export const selectErrorDetalle = createSelector(selectTicketsState, s => s?.errorDetalle ?? null);

export const selectCreando = createSelector(selectTicketsState, s => s?.creando ?? false);
export const selectErrorCrear = createSelector(selectTicketsState, s => s?.errorCrear ?? null);

export const selectCategorias = createSelector(selectTicketsState, s => s?.categorias ?? []);
export const selectAgentes = createSelector(selectTicketsState, s => s?.agentes ?? []);

//Selector derivado para la página actual
export const selectPaginaActual = createSelector(selectFiltros, f => f.pagina);

//Selector derivado para tickets marcados con prioridad
export const selectTicketsPrioridad = createSelector(selectTickets, tickets => 
    tickets.map(t => ({ ...t, prioridadClass: `priority-${t.prioridad}` }))
); 