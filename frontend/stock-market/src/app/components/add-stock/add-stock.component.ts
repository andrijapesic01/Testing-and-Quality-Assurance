import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StockModel } from '../../models/stock.model';
import { Stock } from '../../models/stock';
import { StockService } from '../../services/stock.service';
import { FileService } from '../../services/file.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-stock',
  templateUrl: './add-stock.component.html',
  styleUrl: './add-stock.component.css'
})
export class AddStockComponent {
  stockForm: FormGroup;
  selectedFile!: File;
  uploadImgUrl: string = "";

  constructor(private fb: FormBuilder, private stockService: StockService, private fileService: FileService,
    private router: Router) {
      
    this.stockForm = this.fb.group({
      symbol: ['', Validators.required],
      company: ['', Validators.required],
      currentPrice: [0, [Validators.required, Validators.min(0)]],
      logoUrl: ['', Validators.required],
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    this.uploadImage();
  }

  uploadImage(): void {
    if (this.selectedFile) {
      this.fileService.uploadImage(this.selectedFile).subscribe(
        imageUrl => {
          console.log('Image uploaded successfully. URL:', imageUrl);
          this.uploadImgUrl = imageUrl;
        },
        error => {
          console.error('Error uploading image:', error);
        }
      );
    } 
  }

  submitStock() {
    if (this.stockForm.valid) {
      const stock: StockModel = {
        Symbol: this.stockForm.get('symbol')?.value,
        Company: this.stockForm.get('company')?.value,
        CurrentPrice: this.stockForm.get('currentPrice')?.value,
        LogoURL: this.uploadImgUrl,
        //LogoURL: this.stockForm.get('logoUrl')?.value,
      };

      this.stockService.addStock(stock).subscribe(
        (response: Stock) => {
          console.log('Stock added successfully', response);
          this.router.navigate(['/stocks']);
        },
        (error) => {
          console.error('Error adding portfolio', error);
        }
      );
      //this.stockForm.reset();
    }
  }
}
