import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../models/portfolio';
import { PortfolioService } from '../../services/portfolio.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-portfolios',
  templateUrl: './portfolios.component.html',
  styleUrl: './portfolios.component.css'
})
export class PortfoliosComponent implements OnInit {
  portfolios: Portfolio[] = [];

  constructor(private portfolioService: PortfolioService, private router: Router) {}

  ngOnInit(): void {
    this.loadPortfolios();
  }

  loadPortfolios(): void {
    this.portfolioService.getPortfolios().subscribe((portfolios: Portfolio[]) => {
      this.portfolios = portfolios;
    });
  }

  viewPortfolio(portfolioId: number): void {
    this.router.navigate(['/portfolio', portfolioId]);
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
