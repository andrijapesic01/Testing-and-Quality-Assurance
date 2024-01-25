import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddStockComponent } from './components/add-stock/add-stock.component';
import { AddPortfolioComponent } from './components/add-portfolio/add-portfolio.component';
import { StocksComponent } from './components/stocks/stocks.component';
import { PortfolioComponent } from './components/portfolio/portfolio.component';
import { PortfoliosComponent } from './components/portfolios/portfolios.component';
import { UpdatePortfolioComponent } from './components/update-portfolio/update-portfolio.component';
import { UpdateStockComponent } from './components/update-stock/update-stock.component';

const routes: Routes = [
  {path:'add-stock', component: AddStockComponent},
  {path:'add-portfolio', component: AddPortfolioComponent},
  {path:'home', component: StocksComponent},
  {path:'portfolio/:id', component: PortfolioComponent},
  {path:'update-portfolio/:id', component: UpdatePortfolioComponent},
  {path:'update-stock/:id', component: UpdateStockComponent},
  {path:'portfolios', component: PortfoliosComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
