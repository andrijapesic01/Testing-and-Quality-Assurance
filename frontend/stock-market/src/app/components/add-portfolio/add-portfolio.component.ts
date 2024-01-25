import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioModel } from '../../models/portfolio.model';

@Component({
  selector: 'app-add-portfolio',
  templateUrl: './add-portfolio.component.html',
  styleUrl: './add-portfolio.component.css'
})
export class AddPortfolioComponent implements OnInit {
  portfolioForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.portfolioForm = this.fb.group({
      ownerName: ['', Validators.required],
      bankName: ['', Validators.required],
      bankBalance: [null, [Validators.required, Validators.min(0)]],
      riskTolerance: [null, [Validators.required, Validators.min(0)]],
      investmentStrategy: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.portfolioForm.valid) {
      const formData: PortfolioModel = this.portfolioForm.value;
      console.log('Portfolio Data:', formData);
      this.portfolioForm.reset();
    }
  }
}
