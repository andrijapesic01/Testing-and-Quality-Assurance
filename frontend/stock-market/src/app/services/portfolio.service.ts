import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PortfolioModel } from '../models/portfolio.model';
import { Portfolio } from '../models/portfolio';
import { BuyStockModel } from '../models/buy-stock.model';
import { SellStockModel } from '../models/sell-stock.model';

@Injectable({
  providedIn: 'root'
})
export class PortfolioService {

  apiUrl: string = "https://localhost:7193";
  
  constructor(private http: HttpClient) { }
  
  addPortfolio(portfolioData: PortfolioModel) {
    return this.http.post<Portfolio>(`${this.apiUrl}/Porfolio/AddPortfolio`, portfolioData);
  }

  getPortfolio(id: number) {
    return this.http.get<Portfolio>(`${this.apiUrl}/Portfolio/GetPortfolio/${id}`);
  }

  getPortfolios() {
    return this.http.get<Portfolio[]>(`${this.apiUrl}/Portfolio/GetAllPortfolios`);
  }

  updatePortfolio(id: number, portfolioModel: PortfolioModel) {
    console.log(portfolioModel)
    return this.http.put<Portfolio>(`${this.apiUrl}/Portfolio/UpdatePortfolio/${id}`, portfolioModel);
  }

  deletePortfolio(id: number) {
    return this.http.delete<any>(`${this.apiUrl}/Portfolio/DeletePortfolio/${id}`);
  }

  buyStock(buyStockData: BuyStockModel) {
    return this.http.post<Portfolio>(`${this.apiUrl}/Portfolio/BuyStock`, buyStockData);
  }

  sellStock(sellStockData: SellStockModel) {
    return this.http.post<Portfolio>(`${this.apiUrl}/Portfolio/SellStock`, sellStockData);
  }

}
