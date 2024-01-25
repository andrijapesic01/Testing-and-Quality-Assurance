import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StockModel } from '../../models/stock.model';
import { Stock } from '../../models/stock';
import { StockService } from '../../services/stock.service';

@Component({
  selector: 'app-add-stock',
  templateUrl: './add-stock.component.html',
  styleUrl: './add-stock.component.css'
})
export class AddStockComponent {
  stockForm: FormGroup;

  constructor(private fb: FormBuilder, private stockService: StockService) {
    this.stockForm = this.fb.group({
      symbol: ['', Validators.required],
      company: ['', Validators.required],
      currentPrice: [0, [Validators.required, Validators.min(0)]],
      logoUrl: ['', Validators.required],
    });
  }

  submitStock() {
    if (this.stockForm.valid) {
      const stock: StockModel = {
        Symbol: this.stockForm.get('symbol')?.value,
        Company: this.stockForm.get('company')?.value,
        CurrentPrice: this.stockForm.get('currentPrice')?.value,
        LogoURL: this.stockForm.get('logoUrl')?.value,
      };

      this.stockService.addStock(stock).subscribe(
        (response: Stock) => {
          console.log('Stock added successfully', response);
        },
        (error) => {
          console.error('Error adding portfolio', error);
        }
      );
      this.stockForm.reset();
    }
  }
}
