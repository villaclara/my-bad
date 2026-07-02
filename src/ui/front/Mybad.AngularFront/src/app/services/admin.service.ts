import { HttpClient } from '@angular/common/http';
import { inject, Injectable, OnInit } from '@angular/core';
import { TableDataApiResponse, TableMetaData } from '../models/tables';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private basePath = 'api/data';
  private apiKey: string = '';
  private http = inject(HttpClient);

  getTablesMetaData(): Observable<TableMetaData[]> {
    const url = `${this.basePath}/meta?apiKey=${this.apiKey}`;
    return this.http.get<TableMetaData[]>(url);
  }

  getTableData(tableName: string, targetPage: number, targetSize: number): Observable<TableDataApiResponse> {
    const url = `${this.basePath}/table/${tableName}?page=${targetPage}&size=${targetSize}&apiKey=${this.apiKey}`;
    return this.http.get<TableDataApiResponse>(url);
  }

  setKey(key: string) {
    this.apiKey = key;
  }
}
