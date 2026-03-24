import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { TicketListItem, Categoria, Usuario } from "../../../../core/models"; 
import * as TicketsActions from "../../store/tickets.actions";
import { selectTicketsPrioridad, selectLoadingList, selectErrorList,
    selectTotalRegistros, selectTotalPaginas, selectFiltros, 
    selectCategorias, selectAgentes
 } from "../../store/tickets.selectors";
 
 @Component({
        selector: 'app-ticket-list',
     standalone: false,
        templateUrl: './ticket-list.component.html'
 })

 export class TicketListComponent implements OnInit {
    tickets$: Observable<any[]>;
    loading$: Observable<boolean>;
    error$: Observable<string | null>
    totalRegistros$: Observable<number>;
    totalPaginas$: Observable<number>;
    filtros$: Observable<any>;
    categorias$: Observable<Categoria[]>;
    agentes$: Observable<Usuario[]>;

    displayedColumns: string[] = ['id', 'titulo', 'categoria', 'prioridad', 'estatus', 'agente', 'fecha', 'comentarios'];

    prioridades = ['baja', 'media', 'alta', 'urgente'];
    estatus = ['abierto', 'en_progreso', 'resuelto', 'cerrado'];
        estatusLabels: Record<string, string> = {
            'abierto': 'Abierto',
            'en_progreso': 'En Progreso',
            'resuelto': 'Resuelto',
            'cerrado': 'Cerrado'
        };

    filtroTexto     = '';
    filtroCategoria = '';
    filtroPrioridad = '';
    filtroEstatus   = '';
    filtroAgente    = '';
    filtroActual    = 1;
    paginaActual = 1;

    constructor(private store: Store) {
        this.tickets$ = this.store.select(selectTicketsPrioridad);
        this.loading$ = this.store.select(selectLoadingList);
        this.error$ = this.store.select(selectErrorList);
        this.totalRegistros$ = this.store.select(selectTotalRegistros);
        this.totalPaginas$ = this.store.select(selectTotalPaginas);
        this.filtros$ = this.store.select(selectFiltros);
        this.categorias$ = this.store.select(selectCategorias);
        this.agentes$ = this.store.select(selectAgentes);
    }

    ngOnInit(): void {
        this.store.dispatch(TicketsActions.cargarCatalogos());
        this.filtros$.subscribe(f => {
            this.filtroTexto = f.texto ?? '';
            this.filtroCategoria = f.categoriaId?.toString() ?? '';
            this.filtroPrioridad = f.prioridad ?? '';
            this.filtroEstatus = f.estatus ?? '';
            this.filtroAgente = f.agenteId?.toString() ?? '';
            this.filtroActual = f.pagina ?? 1;
            this.paginaActual = f.pagina ?? 1;
    });
    this.buscar(1);
    }

    buscar(pagina = 1): void {
        const filtros: any = {pagina, registros: 10};
        if (this.filtroTexto) filtros.texto = this.filtroTexto;
        if (this.filtroCategoria) filtros.categoriaId = this.filtroCategoria;
        if (this.filtroPrioridad) filtros.prioridad = this.filtroPrioridad;
        if (this.filtroEstatus) filtros.estatus = this.filtroEstatus;
        if (this.filtroAgente) filtros.agenteId = this.filtroAgente;
        this.store.dispatch(TicketsActions.actualizarFiltros({ filtros }));
        this.store.dispatch(TicketsActions.cargarTickets({ filtros }));
    }

    limpiarFiltros(): void {
        this.filtroTexto = '';
        this.filtroCategoria = '';
        this.filtroPrioridad = '';
        this.filtroEstatus = '';
        this.filtroAgente = '';
        this.buscar(1);
    }  

    cambiarPagina(pagina: number): void {
        this.buscar(pagina);
    }
}
