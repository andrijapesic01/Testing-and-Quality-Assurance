import { Component, OnInit } from '@angular/core';
import { BoughtStock } from '../../models/bought-stock';
import { Portfolio } from '../../models/portfolio';
import { ActivatedRoute, Router } from '@angular/router';
import { PortfolioService } from '../../services/portfolio.service';
import { SellStockModel } from '../../models/sell-stock.model';

@Component({
  selector: 'app-portfolio',
  templateUrl: './portfolio.component.html',
  styleUrl: './portfolio.component.css'
})
export class PortfolioComponent implements OnInit {
  portfolio!: Portfolio;
  boughtStocks: BoughtStock[] = [];

  constructor(private route: ActivatedRoute, private portfolioService: PortfolioService, private router: Router) {}

  ngOnInit(): void {

    const idParam: string | null = this.route.snapshot.paramMap.get('id');
    
    if (idParam !== null) {
      const portfolioId: number = parseInt(idParam, 10);
      this.portfolioService.getPortfolio(portfolioId).subscribe((data: any) => {
        this.portfolio = data.portfolio;
        this.boughtStocks = data.boughtStocks;
      });
    }
  }

  sellStock(boughtStock: BoughtStock): void {
    
    const soldStock: SellStockModel = {
      portfolioId: this.portfolio.id,
      boughtStockId: boughtStock.id,
      quantity: boughtStock.quantity
    };
    this.portfolioService.sellStock(soldStock).subscribe((res) => {
      console.log(res);
      this.router.navigate(['/portfolio', this.portfolio.id]);
    });
  }

  updatePortfolio(portfolioId: number): void {
    this.router.navigate(['/update-portfolio', portfolioId]);
  }

  deletePortfolio(portfolioId: number): void {
    console.log(`Delete portfolio with ID: ${portfolioId}`);
    this.portfolioService.deletePortfolio(portfolioId);
    this.router.navigate(['/portfolios']);
  }
}

