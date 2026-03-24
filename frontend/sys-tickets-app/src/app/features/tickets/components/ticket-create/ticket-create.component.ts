import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import { Categoria } from "../../../../core/models";
import * as TicketsActions from "../../store/tickets.actions";
import { selectCategorias, selectCreando, selectErrorCrear } from "../../store/tickets.selectors";

@Component({
    selector: 'app-ticket-create',
    standalone: false,
    templateUrl: './ticket-create.component.html'
})
export class TicketCreateComponent implements OnInit {
    form: FormGroup;
    categorias$: Observable<Categoria[]>;
    creando$: Observable<boolean>;
    error$: Observable<string | null>;

    prioridades = ['baja', 'media', 'alta', 'urgente'];

    constructor(private fb: FormBuilder, private store: Store) {
        this.form = this.fb.group({
            titulo: ['', [Validators.required, Validators.maxLength(200)]],
            descripcion: ['', Validators.required],
            categoriaId: [null, Validators.required],
            prioridad: ['media', Validators.required]
        });
        this.categorias$ = this.store.select(selectCategorias);
        this.creando$ = this.store.select(selectCreando);
        this.error$ = this.store.select(selectErrorCrear);
    }

    ngOnInit(): void {
        this.store.dispatch(TicketsActions.cargarCatalogos());
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return; 
        }
        const { titulo, descripcion, categoriaId, prioridad } = this.form.value;
        this.store.dispatch(TicketsActions.crearTicket({titulo, descripcion, categoriaId, prioridad }));
    }
}