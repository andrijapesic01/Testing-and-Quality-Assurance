import { Component, OnInit } from '@angular/core';
import { StockService } from '../../services/stock.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Stock } from '../../models/stock';
import { StockModel } from '../../models/stock.model';

@Component({
  selector: 'app-update-stock',
  templateUrl: './update-stock.component.html',
  styleUrl: './update-stock.component.css'
})
export class UpdateStockComponent implements OnInit {
  stockForm!: FormGroup;
  stockId!: number;
  stock!: Stock;
  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private stockService: StockService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const idParam: string | null = this.route.snapshot.paramMap.get('id');

    if (idParam !== null) {
      this.stockId = parseInt(idParam, 10);

      this.stockForm = this.fb.group({
        symbol: ['', Validators.required],
        company: ['', Validators.required],
        currentPrice: [0, [Validators.required, Validators.min(0)]],
        logoUrl: ['', Validators.required],
      });

      this.loadStockData();
    }
  }

  loadStockData() {
    this.stockService.getStock(this.stockId).subscribe(
      (existingStock: Stock) => {
        this.stockForm.patchValue(existingStock);
      },
      (error) => {
        console.error('Error loading stock data', error);
      }
    );
  }

  submitStock() {
    if (this.stockForm.valid) {
      const formData: StockModel = this.stockForm.value;

      this.stockService.updateStock(this.stockId, formData).subscribe(
        (response: Stock) => {
          console.log('Stock updated successfully', response);
        },
        (error) => {
          console.error('Error updating stock', error);
        }
      );
    }
  }
}
