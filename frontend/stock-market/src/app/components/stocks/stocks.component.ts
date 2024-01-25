import { Component, OnInit } from '@angular/core';
import { Stock } from '../../models/stock';
import { Portfolio } from '../../models/portfolio';
import { StockService } from '../../services/stock.service';
import { PortfolioService } from '../../services/portfolio.service';
import { BuyStockModel } from '../../models/buy-stock.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.css'
})
export class StocksComponent implements OnInit {

  stocks: Stock[] = [];
  portfolios: Portfolio[] = []; 
  selectedPortfolio: Portfolio | undefined;
  quantity: number = 1;

  constructor(private stocksService: StockService, private portfoliosService: PortfolioService, 
    private router: Router) { }

  ngOnInit(): void {
    this.loadStocks();
    this.loadPortfolios();
  }

  loadStocks() {
    this.stocksService.getStocks().subscribe((stocks: Stock[])=> {
      this.stocks = stocks;
      console.log(stocks);
    });
  }

  loadPortfolios() {
    this.portfoliosService.getPortfolios().subscribe((portfolios: Portfolio[]) => 
      {this.portfolios = portfolios
      console.log(this.portfolios)}
    ); 
  }

  buyStock(stock: Stock) {

    if(!this.selectedPortfolio || this.quantity < 1 || this.selectedPortfolio.bankBalance < this.quantity * stock.currentPrice)
      return;
    
    console.log(`Buying ${this.quantity} shares of ${stock.symbol} for Portfolio ${this.selectedPortfolio}`);
    
    const buyStock: BuyStockModel = {
      portfolioId: this.selectedPortfolio.id,
      stockId: stock.id,
      quantity: this.quantity
    }

    this.portfoliosService.buyStock(buyStock).subscribe((res)=> console.log(res));

    this.selectedPortfolio = undefined;
    this.quantity = 1;
  }

  deleteStock(stockId: number) {
    this.stocksService.deleteStock(stockId).subscribe((response) => {
      console.log(response)
      this.router.navigate(['/stocks']);
    })
  }

  updateStock(stockId: number) {
    this.router.navigate(['/update-stock', stockId]);
  }

  getFullImageUrl(stock: Stock) {
    return `https://localhost:7193${stock.logoURL}`;
  }

}
