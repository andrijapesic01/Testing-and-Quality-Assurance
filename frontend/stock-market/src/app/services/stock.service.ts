import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StockModel } from '../models/stock.model';
import { Stock } from '../models/stock';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  apiUrl: string = "https://localhost:7193";
  
  constructor(private http: HttpClient) { }

  addStock(stock: StockModel) {
    return this.http.post<Stock>(`${this.apiUrl}/Stock/AddStock`, stock);
  }

  getStock(id: number) {
    return this.http.get<Stock>(`${this.apiUrl}/Stock/GetStock/${id}`);
  }

  getStocks() {
    return this.http.get<Stock[]>(`${this.apiUrl}/Stock/GetAllStocks`);
  }

  updateStock(id: number, stockModel: StockModel) {
    return this.http.put<Stock>(`${this.apiUrl}/Stock/UpdateStock/${id}`, stockModel);
  }

  deleteStock(id: number) {
    return this.http.delete<any>(`${this.apiUrl}/Stock/DeleteStock/${id}`);
  }
}
