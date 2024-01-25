import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { AngularMaterialModule } from './angular-material.module';
import { AddStockComponent } from './components/add-stock/add-stock.component';
import { AddPortfolioComponent } from './components/add-portfolio/add-portfolio.component';
import { StocksComponent } from './components/stocks/stocks.component';
import { PortfoliosComponent } from './components/portfolios/portfolios.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { PortfolioComponent } from './components/portfolio/portfolio.component';
import { UpdatePortfolioComponent } from './components/update-portfolio/update-portfolio.component';
import { UpdateStockComponent } from './components/update-stock/update-stock.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    AddStockComponent,
    AddPortfolioComponent,
    StocksComponent,
    PortfoliosComponent,
    PortfolioComponent,
    UpdatePortfolioComponent,
    UpdateStockComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
