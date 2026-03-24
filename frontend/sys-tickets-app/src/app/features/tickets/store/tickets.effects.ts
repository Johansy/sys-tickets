import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Store } from "@ngrx/store";    
import { forkJoin, of } from "rxjs";
import { catchError, map, switchMap, tap, withLatestFrom, concatMap } from "rxjs/operators";
import { TicketsService } from "../../../core/services/tickets.service";
import * as TicketsActions from "./tickets.actions";
import { selectTicketSeleccionado } from "./tickets.selectors";

@Injectable()
export class TicketsEffects {
  private readonly actions$ = inject(Actions);
  private readonly services = inject(TicketsService);
  private readonly store = inject(Store);
  private readonly router = inject(Router);

    //Cambiar lectura a switchMap para cancelar solicitudes anteriores
    cargarTickets$ = createEffect(() => this.actions$.pipe(
        ofType(TicketsActions.cargarTickets),
        switchMap(({ filtros }) => this.services.buscar(filtros).pipe(
            map(result => TicketsActions.cargarTicketsSuccess({ 
                tickets: result.items, 
                totalRegistros: result.totalRegistros, 
                totalPaginas: result.totalPaginas 
            })),
            catchError(error => [TicketsActions.cargarTicketsFailure({ error: error.message || 'Error desconocido' })])
        ))
    ));

    loadTicketDetalle$ = createEffect(() => this.actions$.pipe(
        ofType(TicketsActions.cargarDetalle),
        switchMap(({ id }) => this.services.obtenerDetalle(id).pipe(
            map(ticket => TicketsActions.cargarDetalleSuccess({ ticket })),
            catchError(error => [TicketsActions.cargarDetalleFailure({ error: error.message || 'Error desconocido' })])
        ))
    ));

    //Cambiar estatus
    cambiarEstatus$ = createEffect(() => this.actions$.pipe(
        ofType(TicketsActions.cambiarEstatus),
        switchMap(({ id, estatus }) => this.services.cambiarEstatus(id, estatus).pipe(
            map(() => TicketsActions.cambiarEstatusSuccess({ id, estatus })),
            catchError(error => [TicketsActions.cambiarEstatusFailure({ error: error.message || 'Error desconocido' })])
        ))
    ));

    //Recargar detalle después de cambiar estatus
    recargarDetalle$ = createEffect(() => this.actions$.pipe(
        ofType(TicketsActions.cambiarEstatusSuccess),
        withLatestFrom(this.store.select(selectTicketSeleccionado)),
        map(([, ticket])=> TicketsActions.cargarDetalle({ id: ticket?.id! }))
    ));

    
  asignarAgente$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TicketsActions.asignarAgente),
      concatMap(({ id, agenteId }) =>
        this.services.asignarAgente(id, agenteId).pipe(
          map(() => TicketsActions.asignarAgenteSuccess()),
          catchError(err => of(TicketsActions.asignarAgenteFailure({ error: err.error?.message ?? 'Error al asignar agente' })))
        )
      )
    )
  );

  reloadAfterAsignarAgente$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TicketsActions.asignarAgenteSuccess),
      withLatestFrom(this.store.select(selectTicketSeleccionado)),  
      map(([, ticket]) => TicketsActions.cargarDetalle({ id: ticket?.id! }))
    )
  );

  addComment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TicketsActions.agregarComentario),
      concatMap(({ id, contenido, esInterno }) =>
        this.services.agregarComentario(id, contenido, esInterno).pipe(
          map(() => TicketsActions.agregarComentarioSuccess()),
          catchError(err => of(TicketsActions.agregarComentarioFailure({ error: err.error?.message ?? 'Error al agregar comentario' })))
        )
      )
    )
  );

  createTicket$ = createEffect(() => this.actions$.pipe(
    ofType(TicketsActions.crearTicket),
    concatMap(payload => this.services.crear(payload).pipe(
      map(response => TicketsActions.crearTicketSuccess({ id: response.id })),
      catchError(err => of(TicketsActions.crearTicketFailure({ error: err.error?.message ?? 'Error al crear ticket' })))
    ))
  ));

  redirecAfterCreate$ = createEffect(() => this.actions$.pipe(
    ofType(TicketsActions.crearTicketSuccess),
    tap(({ id }) => this.router.navigate(['/tickets', id]))
  ), { dispatch: false });

  loadCatalogos$ = createEffect(() => this.actions$.pipe(
    ofType(TicketsActions.cargarCatalogos),
    switchMap(() => forkJoin({
        categorias: this.services.getCategorias(), 
        agentes: this.services.getAgentes()
    }).pipe(
        map(({ categorias, agentes }) => TicketsActions.cargarCatalogosSuccess({ categorias, agentes })),
        catchError(() => of(TicketsActions.cargarCatalogosSuccess({ categorias: [], agentes: [] })))
    ))
  ));
}