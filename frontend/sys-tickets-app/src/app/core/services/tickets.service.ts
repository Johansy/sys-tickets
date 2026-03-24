import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginateResult, TicketDetalle, TicketListItem,
    Categoria, Usuario, TicketFiltros
 } from '../models';

@Injectable({providedIn: 'root'})
export class TicketsService {
    private readonly apiUrl = environment.apiUrl;
    //Simula el usuario actual - en producción esto vendría del servicio de autenticación
    private usuarioId = 1; // ID del usuario actual (simulado)
    constructor (private http: HttpClient) {}
    private get headers() {
        return { 'X-Usuario-Id': this.usuarioId.toString() };
    }
    buscar(filtros: TicketFiltros): Observable<PaginateResult<TicketListItem>> {
        let params = new HttpParams()
        .set('pagina', filtros.pagina)
        .set('registros', filtros.registros);
        if (filtros.texto) params = params.set('texto', filtros.texto);
        if (filtros.categoriaId) params = params.set('categoriaId', filtros.categoriaId);
        if (filtros.prioridad) params = params.set('prioridad', filtros.prioridad);
        if (filtros.estatus) params = params.set('estatus', filtros.estatus);
        if (filtros.agenteId) params = params.set('agenteId', filtros.agenteId);
        return this.http.get<PaginateResult<TicketListItem>>(`${this.apiUrl}/tickets`, { params, headers: this.headers });
    }
    obtenerDetalle(id: number): Observable<TicketDetalle> {
        return this.http.get<TicketDetalle>(`${this.apiUrl}/tickets/${id}`, { headers: this.headers });
    }
    crear(data:{titulo: string, descripcion: string, categoriaId: number, prioridad: string}) {
        return this.http.post<{id: number}>(
            `${this.apiUrl}/tickets`, data, { headers: this.headers }
        );
    }
    
    cambiarEstatus(id: number, estatus: string) {
        return this.http.patch(
            `${this.apiUrl}/tickets/${id}/estatus`, { estatus }, { headers: this.headers }
        );
    }
    asignarAgente(id: number, agenteId: number) {
        return this.http.patch(
            `${this.apiUrl}/tickets/${id}/asignar`, { agenteId }, { headers: this.headers }
        );
    }
    agregarComentario(id: number, contenido: string, esInterno: boolean) {
        return this.http.post<{id: number}>(
            `${this.apiUrl}/tickets/${id}/comentarios`, { contenido, esInterno}, { headers: this.headers }
        );
    }
    getCategorias(): Observable<Categoria[]> {
        return this.http.get<Categoria[]>(`${this.apiUrl}/catalogos/categorias`);
    }
    getAgentes(): Observable<Usuario[]> {
        return this.http.get<Usuario[]>(`${this.apiUrl}/catalogos/agentes`);
    }
}