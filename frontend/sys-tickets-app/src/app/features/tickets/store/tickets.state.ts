import { TicketDetalle, TicketListItem, TicketFiltros, Usuario, Categoria } from "../../../core/models";

export interface TicketsState {
    //Listado
    tickets: TicketListItem[];
    filtros: TicketFiltros;
    totalRegistros: number;
    totalPaginas: number;
    loadingList: boolean;
    errorList: string | null;

    //Detalle
    ticketSeleccionado: TicketDetalle | null;
    loadingDetalle: boolean;
    errorDetalle: string | null;

    //Crear
    creando: boolean;
    errorCrear: string | null;

    //Catálogos
    categorias: Categoria[];
    agentes: Usuario[];
}

export const initialState: TicketsState = {
    filtros: { pagina: 1, registros: 10 },
    tickets: [],
    totalRegistros: 0,
    totalPaginas: 0,
    loadingList: false,
    errorList: null,
    ticketSeleccionado: null,
    loadingDetalle: false,
    errorDetalle: null,
    creando: false,
    errorCrear: null,
    categorias: [],
    agentes: []
}