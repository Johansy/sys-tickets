import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

// Angular Material
import { MatToolbarModule }     from '@angular/material/toolbar';
import { MatButtonModule }      from '@angular/material/button';
import { MatCardModule }        from '@angular/material/card';
import { MatFormFieldModule }   from '@angular/material/form-field';
import { MatInputModule }       from '@angular/material/input';
import { MatSelectModule }      from '@angular/material/select';
import { MatTableModule }       from '@angular/material/table';
import { MatIconModule }        from '@angular/material/icon';
import { MatChipsModule }       from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule }     from '@angular/material/divider';
import { MatCheckboxModule }    from '@angular/material/checkbox';

import { ticketsReducer }        from './store/tickets.reducer';
import { TicketsEffects }        from './store/tickets.effects';
import { TicketsRoutingModule }  from './tickets-routing.module';

import { TicketListComponent }   from './components/ticket-list/ticket-list.component';
import { TicketDetailComponent } from './components/ticket-detail/ticket-detail.component';
import { TicketCreateComponent } from './components/ticket-create/ticket-create.component';

@NgModule({
  declarations: [
    TicketListComponent,
    TicketDetailComponent,
    TicketCreateComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TicketsRoutingModule,
    StoreModule.forFeature('tickets', ticketsReducer),
    EffectsModule.forFeature([TicketsEffects]),
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    MatChipsModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatCheckboxModule,
  ]
})
export class TicketsModule {}
