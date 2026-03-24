import { Routes } from '@angular/router';

export const routes: Routes = [
	{ path: '', redirectTo: 'tickets', pathMatch: 'full' },
	{
		path: 'tickets',
		loadChildren: () =>
			import('./features/tickets/tickets.module').then(m => m.TicketsModule)
	},
	{ path: '**', redirectTo: 'tickets' }
];
