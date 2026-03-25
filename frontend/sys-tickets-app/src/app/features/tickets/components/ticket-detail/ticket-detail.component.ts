import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';   
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Estatus, TicketDetalle, Usuario } from '../../../../core/models';
import * as TicketsActions from '../../store/tickets.actions';
import { selectTicketSeleccionado, selectLoadingDetalle, 
    selectErrorDetalle, selectAgentes, selectCategorias 
} from '../../store/tickets.selectors';

@Component({
    selector: 'app-ticket-detail',
    standalone: false,
    templateUrl: './ticket-detail.component.html'
})
export class TicketDetailComponent implements OnInit {
    ticket$: Observable<TicketDetalle | null>;
    loading$: Observable<boolean>;
    error$: Observable<string | null>;
    agentes$: Observable<Usuario[]>;
    mensajeOperacion: string | null = null;

    nuevoComentario = '';
    esInterno = false;
    nuevoEstatus: Estatus | '' = '';
    agenteSeleccionado = '';
        estatus: Estatus[] = ['abierto', 'en_progreso', 'resuelto', 'cerrado'];
        estatusLabels: Record<Estatus, string> = {
            'abierto': 'Abierto',
            'en_progreso': 'En Progreso',
            'resuelto': 'Resuelto',
            'cerrado': 'Cerrado'
        };

    constructor(private store: Store, private route: ActivatedRoute) {
        this.ticket$ = this.store.select(selectTicketSeleccionado);
        this.loading$ = this.store.select(selectLoadingDetalle);
        this.error$ = this.store.select(selectErrorDetalle);
        this.agentes$ = this.store.select(selectAgentes);
    }

    ngOnInit(): void {
        const id = Number(this.route.snapshot.paramMap.get('id'));
        this.store.dispatch(TicketsActions.cargarCatalogos());
        this.store.dispatch(TicketsActions.cargarDetalle({ id }));
    }

    cambiarEstatus(ticketDetalle: TicketDetalle): void {
        if (!this.nuevoEstatus) return;

        this.mensajeOperacion = null;
        this.store.dispatch(TicketsActions.cambiarEstatus({ id: ticketDetalle.id, estatus: this.nuevoEstatus }));
        this.nuevoEstatus = '';
    }

    asignarAgente(ticketDetalle: TicketDetalle): void {
        if (!this.agenteSeleccionado) return;
        this.store.dispatch(TicketsActions.asignarAgente({ id: ticketDetalle.id, agenteId: Number(this.agenteSeleccionado) }));
        this.agenteSeleccionado = '';
    }

    enviarComentario(ticketDetalle: TicketDetalle): void {
        if (!this.nuevoComentario.trim()) return;
        this.store.dispatch(TicketsActions.agregarComentario({
            id: ticketDetalle.id,
            contenido: this.nuevoComentario.trim(),
            esInterno: this.esInterno
        }));
        this.nuevoComentario = '';
        this.esInterno = false;
    }
}